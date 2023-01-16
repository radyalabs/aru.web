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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Trisatech.MWorkforce.Cms.Controllers
{
    [Authorize(Roles = "SA")]
    public class NewsController : Controller
    {
        private readonly IContentManagementService newsService;
        public NewsController(IContentManagementService newsService)
        {
            this.newsService = newsService;
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.SelectedMenu = "news";

            base.OnActionExecuted(context);
        }
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Details(string id)
        {
            var newsDetail = new NewsViewModel();
            var result = newsService.Get(id);
            if (result != null)
                CopyProperty.CopyPropertiesTo(result, newsDetail);

            return View(newsDetail);
        }

        public IActionResult Create()
        {
            NewsViewModel model = new NewsViewModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm]NewsViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);

                    NewsModel news = new NewsModel();
                    CopyProperty.CopyPropertiesTo(model, news);

                    news.NewsId = Guid.NewGuid().ToString();
                    news.PublishedDate = model.PublishedDate.ToUniversalTime();

                    newsService.Add(news, userAuth.UserId, false);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            NewsViewModel model = new NewsViewModel();

            try
            {
                if (string.IsNullOrEmpty(id))
                    return NotFound();

                var news = newsService.Get(id);
                if (news == null)
                    return NotFound();

                CopyProperty.CopyPropertiesTo(news, model);
            }
            catch (Exception ex)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, NewsViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);

                    NewsModel news = new NewsModel();
                    CopyProperty.CopyPropertiesTo(model, news);
                    news.NewsId = id;

                    newsService.Edit(news, userAuth.UserId);
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
            if (ModelState.IsValid)
            {
                try
                {
                    var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);
                    newsService.Delete(id, userAuth.UserId);
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
