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
using Trisatech.AspNet.Common.Helpers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Trisatech.MWorkforce.Cms.Controllers
{
    [Authorize(Roles = "SA,OPR")]
    public class OutletController : Controller
    {
        private readonly ICustomerService customerService;
        public OutletController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.SelectedMenu = "outlet";

            base.OnActionExecuted(context);
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Details(string id)
        {
            var outletDetail = new OutletViewModel();
            var result = customerService.detailCust(id);
            if (result != null)
                CopyProperty.CopyPropertiesTo(result, outletDetail);
            else
                NotFound();
            return View(outletDetail);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            OutletViewModel model = new OutletViewModel();

            try
            {
                if (string.IsNullOrEmpty(id))
                    return NotFound();

                var outlet = customerService.detailCust(id);
                if (outlet == null)
                    return NotFound();

                CopyProperty.CopyPropertiesTo(outlet, model);
            }
            catch
            {
                return NotFound();
            }

            var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);
            if (userAuth.RoleCode == "SA")
                ViewBag.CustomerList = customerService.GetCustomers("", 10, 0, "", "", null, null);
            else
                ViewBag.CustomerList = customerService.GetCustomers("", 10, 0, "", "", null, null, userAuth);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, OutletViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);

                    CustomerDetailModel outlet = new CustomerDetailModel();
                    CopyProperty.CopyPropertiesTo(model, outlet);
                    outlet.CustomerId = id;

                    customerService.EditOutlet(outlet, userAuth.UserId);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                }
            }

            return View(model);
        }

        public IActionResult Print(string id)
        {
            return NotFound();
            /*
            string footer = "--footer-right \"Date: [date] [time]\" " + "--footer-center \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\"";
            var outletDetail = new OutletViewModel();
            var result = customerService.detailCust(id);
            if (result != null)
                CopyProperty.CopyPropertiesTo(result, outletDetail);
            else
                NotFound();
            return new ViewAsPdf("print", outletDetail)
            {
                FileName = "Outlet Info_"+ outletDetail.StoreName +"_"+ DateTime.Now +".pdf",
                Model = outletDetail,
                CustomSwitches = footer
            };
            */
        }

        public IActionResult Create()
        {
            OutletViewModel model = new OutletViewModel();
            var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);
            if (userAuth.RoleCode == "SA")
                ViewBag.CustomerList = customerService.GetCustomers("", 10, 0, "", "", null, null);
            else
                ViewBag.CustomerList = customerService.GetCustomers("", 10, 0, "", "", null, null,userAuth);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm]OutletViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);

                    CustomerDetailModel outlet = new CustomerDetailModel();
                    CopyProperty.CopyPropertiesTo(model, outlet);

                    customerService.AddCustDetail(outlet, userAuth.UserId);
                    ViewBag.CustomerList = customerService.GetCustomers("", 10, 0, "", "", null, null);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
