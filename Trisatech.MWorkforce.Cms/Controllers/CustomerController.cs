using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trisatech.MWorkforce.Business.Entities;
using Trisatech.MWorkforce.Business.Interfaces;
using Trisatech.MWorkforce.Cms.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Trisatech.AspNet.Common.Extensions;
using Trisatech.AspNet.Common.Helpers;

namespace Trisatech.MWorkforce.Cms.Controllers
{
    [Authorize(Roles = "SA,OPR")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService customerService;
        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.SelectedMenu = "customer";

            base.OnActionExecuted(context);
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Details(string id)
        {
            var userDetail = new CustomerViewModel();
            var result = customerService.Get(id);
            if (result != null)
                CopyProperty.CopyPropertiesTo(result, userDetail);

            return View(userDetail);
        }

        public IActionResult Create()
        {
            CustomerViewModel model = new CustomerViewModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm]CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);

                    CustomerModel customer = new CustomerModel();
                    CopyProperty.CopyPropertiesTo(model, customer);

                    customer.CustomerId = Guid.NewGuid().ToString();
                    customer.CustomerCode = $"{model.CustomerName.GetInitial()}-{customer.CustomerId.Substring(0,4).ToUpper()}";
                    
                    if (model.SaveAsContact)
                    {
                        customer.Contacts = new List<ContactModel>();
                        customer.Contacts.Add(new ContactModel
                        {
                            ContactId = Guid.NewGuid().ToString(),
                            ContactName = model.CustomerName,
                            ContactNumber = model.CustomerPhoneNumber,
                            CustomerCode = customer.CustomerCode
                        });
                    }
                    
                    customerService.Add(customer, userAuth.UserId, model.SaveAsContact);

                    return RedirectToAction(nameof(Index));
                }
                catch{ throw; }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(string id) 
        {
            CustomerViewModel model = new CustomerViewModel();

            try
            {
                if (string.IsNullOrEmpty(id))
                    return NotFound();

                var customer = customerService.Get(id);
                if (customer == null)
                    return NotFound();

                CopyProperty.CopyPropertiesTo(customer, model);
            }
            catch { throw; }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);

                    CustomerModel customer = new CustomerModel();
                    CopyProperty.CopyPropertiesTo(model, customer);
                    customer.CustomerId = id;
                    
                    customerService.Edit(customer, userAuth.UserId);
                    return RedirectToAction(nameof(Index));
                }
                catch { throw; }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);
                    customerService.Delete(id, userAuth.UserId);
                }
                catch { throw; }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}