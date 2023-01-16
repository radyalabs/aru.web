using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Trisatech.MWorkforce.Cms.ViewModels;
using Trisatech.MWorkforce.Business.Services;
using Trisatech.MWorkforce.Business.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Trisatech.MWorkforce.Cms.Models;
using Trisatech.AspNet.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Trisatech.MWorkforce.Cms.Helpers;

namespace Trisatech.MWorkforce.Cms.Controllers
{
    public class AccountController : Controller
    {
        private const string INDEX = "/Home/Index";
        private readonly IAccountService accountService;
        private readonly ApplicationSetting applicationSetting;
        public AccountController(IAccountService accountService, IOptions<ApplicationSetting> applicationSetting)
        {
            this.accountService = accountService;
            this.applicationSetting = applicationSetting.Value;
        }

        public IActionResult Login()
        {
            LoginViewModel viewModel = new LoginViewModel();
            
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    loginViewModel.Password = loginViewModel.Password.ToSHA256();
                    var authResult = accountService.Login(loginViewModel.Username, loginViewModel.Password.ToSHA256());
                    AppCookieHelper.Set(authResult, authResult.RoleCode, loginViewModel.IsRemember, applicationSetting.ProjectIdentityName, this.HttpContext);

                    return RedirectToAction("Index", "Home");
                }
                catch(Exception ex)
                {
                    ViewData["ErrorMessage"] = ex.Message;

                    return View(loginViewModel);
                }
            }
            else
            {
                return View(loginViewModel);
            }
        }
        
        public IActionResult Logout()
        {
            Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.LogOut(this.HttpContext);

            return RedirectToAction("Login");
        }
    }
}