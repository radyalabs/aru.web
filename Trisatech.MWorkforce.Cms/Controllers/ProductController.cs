using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trisatech.MWorkforce.Business.Entities;
using Trisatech.MWorkforce.Business.Interfaces;
using Trisatech.MWorkforce.Cms.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Trisatech.AspNet.Common.Helpers;

namespace Trisatech.MWorkforce.Cms.Controllers
{
    [Authorize(Roles = "SA")]
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.SelectedMenu = "product";

            base.OnActionExecuted(context);
        }

        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        // GET: Product/Details/5
        public ActionResult Details(string id)
        {
            ProductViewModel viewModel = new ProductViewModel();
            try
            {
                var productModel = productService.Detail(id);
                CopyProperty.CopyPropertiesTo(productModel, viewModel);
                viewModel.ProductModel = productModel.ProductType;

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return View(viewModel);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            ProductViewModel viewModel = new ProductViewModel();

            return View(viewModel);
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);

                    ProductModel productModel = new ProductModel();
                    CopyProperty.CopyPropertiesTo(collection, productModel);
                    productModel.ProductId = Guid.NewGuid().ToString();
                    productModel.ProductType = collection.ProductModel;

                    productService.Add(productModel, userAuth.UserId);

                    return RedirectToAction(nameof(Index));
                }
                catch(Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                    return View(collection);
                }
            }

            return View(collection);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(string id)
        {
            ProductViewModel viewModel = new ProductViewModel();
            try
            {
                var productModel = productService.Detail(id);
                CopyProperty.CopyPropertiesTo(productModel, viewModel);
                viewModel.ProductModel = productModel.ProductType;

            }catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return View(viewModel);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, ProductViewModel collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);

                    ProductModel productModel = new ProductModel();
                    CopyProperty.CopyPropertiesTo(collection, productModel);
                    productModel.ProductId = id;
                    productModel.ProductType = collection.ProductModel;

                    productService.Edit(productModel, userAuth.UserId);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                    return View(collection);
                }
            }

            return View(collection);
        }

        // GET: Product/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: Product/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            try
            {
                // TODO: Add delete logic here
                var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);
                productService.Delete(id, userAuth.UserId);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}