using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trisatech.MWorkforce.Business.Entities;
using Trisatech.MWorkforce.Business.Interfaces;
using Trisatech.MWorkforce.Cms.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Trisatech.AspNet.Common.Extensions;
using Trisatech.AspNet.Common.Helpers;

namespace Trisatech.MWorkforce.Cms.Controllers
{
    public class ContactController : Controller
    {
        private readonly ICustomerService customerService;
        public ContactController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.SelectedMenu = "contact";

            base.OnActionExecuted(context);
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Details(string id)
        {
            var userDetail = new ContactViewModel();
            var result = customerService.GetContact(id);
            if (result != null)
                CopyProperty.CopyPropertiesTo(result, userDetail);

            return View(userDetail);
        }

        public IActionResult Create()
        {
            ContactViewModel model = new ContactViewModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);

                    ContactModel contact = new ContactModel();
                    CopyProperty.CopyPropertiesTo(model, contact);
                    contact.ContactId = Guid.NewGuid().ToString();
                    contact.ContactNumber = model.ContactNumber.MobilePhoneFormat();
                    customerService.AddContact(contact, userAuth.UserId);

                    return RedirectToAction(nameof(Index));
                }
                catch { throw; }
            }

            return View(model);
        }

        public IActionResult Edit(string id)
        {
            ContactViewModel contact = new ContactViewModel();

            try
            {
                if (string.IsNullOrEmpty(id))
                    return NotFound();

                var result = customerService.GetContact(id);
                CopyProperty.CopyPropertiesTo(result, contact);
            }
            catch { throw; }

            return View(contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);

                    ContactModel contact = new ContactModel();
                    CopyProperty.CopyPropertiesTo(model, contact);
                    contact.ContactId = id;
                    contact.ContactNumber = contact.ContactNumber.MobilePhoneFormat();

                    customerService.EditContact(contact, userAuth.UserId);

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
                    
                    customerService.DeleteContact(id, userAuth.UserId);
                }
                catch { throw; }
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}