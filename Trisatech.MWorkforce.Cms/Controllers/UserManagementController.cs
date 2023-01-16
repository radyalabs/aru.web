using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using Trisatech.MWorkforce.Business.Entities;
using Trisatech.MWorkforce.Business.Interfaces;
using Trisatech.MWorkforce.Domain.Entities;
using Trisatech.MWorkforce.Cms.Helpers;
using Trisatech.MWorkforce.Cms.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Trisatech.AspNet.Common.Extensions;
using Trisatech.AspNet.Common.Helpers;

namespace Trisatech.MWorkforce.Cms.Controllers
{
    [Authorize(Roles = "SA")]
    public class UserManagementController : Controller
    {

		private readonly ITerritoryService territoryService;
		private readonly IUserManagementService _service;
        public UserManagementController(IUserManagementService userManagementService, ITerritoryService territoryService)
        {
            _service = userManagementService;
			this.territoryService = territoryService;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.SelectedMenu = "users";
            base.OnActionExecuted(context);
        }

        // GET: UserManagment
        public ActionResult Index()
        {

            return View();
        }

        // GET: UserManagment/Details/5
        public ActionResult Details(string id)
        {
            var userDetail = new UserModel();
            var result = _service.DetailUser(id);
            if(result != null)
                CopyProperty.CopyPropertiesTo(result, userDetail);

            return View(userDetail);
        }

        // GET: UserManagment/Create
        public ActionResult Create()
        {
            var roles = _service.GetRoles();
            UserRegisterViewModel model = new UserRegisterViewModel();

            ViewBag.UserRoles = new SelectList(roles, "RoleId", "RoleName");
			ViewBag.TerritoryList = territoryService.Get().ToList();

			return View(model);
        }

        // POST: UserManagment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserRegisterViewModel collection)
        {
            var roles = _service.GetRoles();

            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add insert logic here
                    var userAuth = AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);

					UserModel newUser = new UserModel();
                    CopyProperty.CopyPropertiesTo(collection, newUser);
                    newUser.Name = collection.Name;
                    newUser.AccountId = Guid.NewGuid().ToString();
                    newUser.UserId = Guid.NewGuid().ToString();
                    newUser.Password = collection.Password.ToSHA256().ToSHA256();
                    newUser.RoleCode = roles.Where(x => x.RoleId == collection.RoleId).FirstOrDefault().RoleCode;
                    
					_service.Add(newUser, userAuth.UserId, true);

                    return RedirectToAction(nameof(Index));
                }catch(ApplicationException appEx)
                {
                    ViewBag.ErrorMessage = appEx.Message;
                }
                catch(Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                }
            }
            
            ViewBag.UserRoles = new SelectList(roles, "RoleId", "RoleName");
			ViewBag.TerritoryList = territoryService.Get().ToList();

			return View(collection);
        }

        // GET: UserManagment/Edit/5
        public ActionResult Edit(string id)
        {
            var userDetail = new UpdateUserViewModel();

            try
            {
                var result = _service.DetailUser(id);
                if (result != null)
                {
                    CopyProperty.CopyPropertiesTo(result, userDetail);
                    userDetail.Territoryid = result.UserTerritories.FirstOrDefault()?.TerritoryId;
                }
            }
            catch(Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }


            var territories = territoryService.Get().ToList();

            ViewBag.UserRoles = new SelectList(_service.GetRoles(), "RoleId", "RoleName", userDetail.RoleId);
            ViewBag.Territoryid = new SelectList(territories, "TerritoryId", "Name", userDetail.Territoryid);
			ViewBag.TerritoryList = territories;

			return View(userDetail);
        }

        // POST: UserManagment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, UpdateUserViewModel collection)
        {
            var roles = new List<Role>();
            try
            {
                roles = _service.GetRoles();
            }
            catch (Exception ex)
            {
                ViewBag.UserRoles = new SelectList(roles, "RoleId", "RoleName", collection.RoleId);
                ViewBag.Territoryid = new SelectList(roles, "TerritoryId", "Name", collection.Territoryid);
                ViewBag.ErrorMessage = ex.Message;

                return View(collection);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add insert logic here
                    var userAuth = AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);
                    bool isChangedPassword = false;

                    UserModel user = new UserModel();
                    CopyProperty.CopyPropertiesTo(collection, user);
                    user.UserId = id;

                    if (!string.IsNullOrEmpty(collection.Password))
                    {
                        isChangedPassword = true;
                        user.Password = collection.Password.ToSHA256().ToSHA256();
                    }

                    user.RoleCode = roles.FirstOrDefault(x => x.RoleId == collection.RoleId).RoleCode;
                    _service.Edit(user, userAuth.UserId, isChangedPassword);

                    return RedirectToAction(nameof(Index));
                }
                catch (ApplicationException appEx)
                {
                    ViewBag.ErrorMessage = appEx.Message;
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                }
            }

            var territories = territoryService.Get().ToList();

            ViewBag.UserRoles = new SelectList(roles, "RoleId", "RoleName", collection.RoleId);
            ViewBag.Territoryid = new SelectList(territories, "TerritoryId", "Name", collection.Territoryid);
            ViewBag.TerritoryList = territories;


            return View(collection);
        }
        
        // POST: UserManagment/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            try
            {
                // TODO: Add delete logic here
                var userAuth = AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);

                _service.Delete(id, userAuth.UserId);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Details));
            }
        }
    }
}