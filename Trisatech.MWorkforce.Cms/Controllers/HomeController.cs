using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trisatech.MWorkforce.Business.Interfaces;
using Trisatech.MWorkforce.Cms.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Trisatech.MWorkforce.Cms.Interfaces;
using Trisatech.MWorkforce.Cms.ViewModels;
using Microsoft.Extensions.Options;
using Trisatech.MWorkforce.Cms.Helpers;
using Trisatech.MWorkforce.Business.Entities;

namespace Trisatech.MWorkforce.Cms.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ITerritoryService territoryService;
        private readonly IDashboardService dashboardService;
        private readonly ApplicationSetting applicationSetting;
        public HomeController(ITerritoryService territoryService,
            IDashboardService dashboardService, 
            IOptions<ApplicationSetting> option)
        {
            this.territoryService = territoryService;
            this.dashboardService = dashboardService;
            this.applicationSetting = option.Value;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.SelectedMenu = "home";
            base.OnActionExecuted(context);
        }

        public async Task<IActionResult> Index()
        {
            var userAuth = AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);
            string[] regions = GetTerritoryByRoleCode(userAuth);

            DashboardViewModel model = new DashboardViewModel();
            DateTime utcNow = DateTime.UtcNow;
            int totalKunjungan = 0;
            decimal totalInvoice = 0;
            decimal totalPaymeent = 0;
            int totalKunjunganGagal = 0;

            try
            {
                model.AgentLocation = await dashboardService.GetLocationAgent(regions);
                totalKunjungan = await dashboardService.CountTotalTask(regions, utcNow);
                totalKunjunganGagal = await dashboardService.CountTaskFailed(regions, utcNow);
                totalInvoice = await dashboardService.CountTotalInvoice(regions, utcNow);
                totalPaymeent = await dashboardService.CountTotalPayment(regions, utcNow);
            }
            catch { throw; }

            ViewBag.TotalKunjungan = totalKunjungan;
            ViewBag.NominalInvoice = totalInvoice;
            ViewBag.NominalPayment = totalPaymeent;
            ViewBag.KunjunganGagal = totalKunjunganGagal;
            ViewBag.GoogleMapsKey = applicationSetting.GoogleMapsKey;
            
            return View(model);
        }

        private string[] GetTerritoryByRoleCode(UserAuthenticated userAuth)
        {
            string[] territory = Array.Empty<string>();

            if (userAuth.RoleCode == "SA")
            {
                var territoryResult = territoryService.Get();
                if (territoryResult != null && territoryResult.Any())
                    territory = territoryResult.Select(x => x.TerritoryId).ToArray();
            }
            else
            {
                territory = userAuth.TerritoryId.ToArray();
            }

            return territory;
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
