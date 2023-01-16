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
    [Authorize]
    public class SurveyController : Controller
    {
        private readonly ISurveyService surveyService;
        public SurveyController(ISurveyService surveyService)
        {
            this.surveyService = surveyService;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.SelectedMenu = "survey";

            base.OnActionExecuted(context);
        }

        // GET: Survey
        public ActionResult Index()
        {
            return View();
        }

        // GET: Survey/Details/5
        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var result = surveyService.Get(id);
            if (result == null)
                return NotFound();
            
            SurveyViewModel surveyViewModel = new SurveyViewModel();
            CopyProperty.CopyPropertiesTo(result, surveyViewModel);
            surveyViewModel.StartDate.ToLocalTime();
            surveyViewModel.EndDate.ToLocalTime();

            return View(surveyViewModel);
        }

        // GET: Survey/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Survey/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SurveyViewModel collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);

                    // TODO: Add insert logic here
                    SurveyModel surveyModel = new SurveyModel();

                    CopyProperty.CopyPropertiesTo(collection, surveyModel);
                    surveyModel.SurveyId = Guid.NewGuid().ToString();
                    surveyModel.StartDate = collection.StartDate.ToUniversalTime();
                    surveyModel.EndDate = collection.EndDate.ToUniversalTime();

                    surveyService.Add(surveyModel, userAuth.UserId);
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                }
            }

            return View(collection);
        }

        // GET: Survey/Edit/5
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var result = surveyService.Get(id);
            if(result == null)
            {
                return NotFound();
            }

            SurveyViewModel surveyViewModel = new SurveyViewModel();
            CopyProperty.CopyPropertiesTo(result, surveyViewModel);
            surveyViewModel.StartDate.ToLocalTime();
            surveyViewModel.EndDate.ToLocalTime();

            return View(surveyViewModel);
        }

        // POST: Survey/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, SurveyViewModel collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);

                    // TODO: Add update logic here
                    SurveyModel surveyModel = new SurveyModel();
                    CopyProperty.CopyPropertiesTo(collection, surveyModel);

                    surveyModel.SurveyId = id;
                    surveyModel.StartDate = collection.StartDate.ToUniversalTime();
                    surveyModel.EndDate = collection.EndDate.ToUniversalTime();

                    surveyService.Edit(surveyModel, userAuth.UserId);

                    return RedirectToAction(nameof(Index));
                }
                catch(Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                }
            }

            return View(collection);
        }
        
        // POST: Survey/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            try
            {
                var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);

                surveyService.Delete(id, userAuth.UserId);
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}