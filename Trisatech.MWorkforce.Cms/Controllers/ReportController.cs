using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trisatech.MWorkforce.Business.Entities;
using Trisatech.MWorkforce.Business.Interfaces;
using Trisatech.MWorkforce.Cms.Helpers;
using Trisatech.MWorkforce.Cms.Interfaces;
using Trisatech.MWorkforce.Cms.Models;
using Trisatech.MWorkforce.Cms.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Trisatech.MWorkforce.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Trisatech.MWorkforce.Cms.Controllers
{
    [Authorize(Roles = "SA,OPR")]
    public class ReportController : Controller
    {
        private readonly ITaskReportService reportService;
        private readonly IAssignmentReportService assignmentReportService;
        private readonly ApplicationSetting applicationSetting;
        private readonly IUserManagementService userManagementService;
        public ReportController(ITaskReportService reportService, 
            IOptions<ApplicationSetting> options,
            IAssignmentReportService assignmentReportService,
            IUserManagementService userManagementService)
        {
            this.reportService = reportService;
            this.assignmentReportService = assignmentReportService;
            this.userManagementService = userManagementService;
            applicationSetting = options.Value;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.SelectedMenu = "report";

            base.OnActionExecuted(context);
        }

        public async Task<IActionResult> DailyReport(){
            
            var userList = await userManagementService.GetUserByRoles(AppConstant.Role.SALES, AppConstant.Role.DRIVER);
            
            ViewBag.SalesList = userList.Select(x=> new SelectListItem()
            {
                Value = x.UserId,
                Text = $"{x.UserCode} - { x.Name } ({x.RoleCode})"
            }).ToList();

            ViewBag.SelectedMenu = "dailyreport";
            ViewBag.CurrentDate = DateTime.Now.Date;
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DailyReport([Bind("DateInput, SalesId")]DateTime DateInput, string UserId){
            
            try{
                var listOfTask = await assignmentReportService.GetDetail(UserId, DateInput);

                if(listOfTask == null)
                {
                    return Json(new {
                        success = false,
                        status = $"Tidak ada jadwal kunjungan sales di tanggal {DateInput.ToString("dddd, MMM yyyy")}",
                    });
                }

                if(listOfTask.VisitHistory == null || !listOfTask.VisitHistory.Any())
                {
                    return Json(new {
                        success = false,
                        status = $"User ini tidak memiliki kunjungan untuk tanggal {DateInput.ToString("dddd, MMM yyyy")}",
                    });
                }

                var visitHistory = new List<CmsVisitHistoryViewModel>();

                //Making data not duplicate
                foreach(var item in listOfTask.VisitHistory.GroupBy(x=> new { x.CustomerCode, x.CustomerName } )){
                    visitHistory.Add(item.FirstOrDefault());
                }

                listOfTask.VisitHistory = visitHistory;

                return Json(new {
                    success = true,
                    status = "Ok",
                    data = listOfTask
                });
            }catch(Exception ex){
                return Json(new {
                    success = false,
                    status = ex.Message,
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveDailyReport([FromBody]ViewModels.InsertDailyReportViewModel request){
            if(request == null){
                return Json(new {
                    success = false,
                    status = "Data yang anda input salah, sehingga tidak dapat diterima sistem. Silahkan diperiksa kembali"
                });
            }

            if(request.Data == null){
                return Json(new {
                    success = false,
                    status = "Data yang akan disipan tidak boleh kosong"
                });
            }

            var dataGroup = request.Data.GroupBy(x=>x.CustomerCode).ToList();

            if(dataGroup.Count < request.Data.Count){
                var duplicateData = dataGroup.Where(x=>x.Count() > 1).ToList();

                string errorMessage = "Data toko diinput lebih dari satu kali";
                List<string> listOfCustomerCode = new List<string>();
                foreach(var item in duplicateData){
                    listOfCustomerCode.Add(item.Key);
                }

                errorMessage = $"{errorMessage}: {string.Join(",", listOfCustomerCode)}";

                return Json(new {
                    success = false,
                    status = errorMessage
                });
            }

            try{
                
                var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);

                await reportService.InsertManualReport(request, userAuth.UserId);
    
                return Json(new {
                    success = true,
                    status = "Ok"
                });
            }catch(Exception ex) {
                return Json(new {
                    success = false,
                    status = ex.Message
                });
            }
        }

        public IActionResult DailyReportIndex(){

            ViewBag.SelectedMenu = "dailyreport";
            ViewBag.CurrentDate = DateTime.UtcNow.ToLocalTime().ToString("MM/dd/yyyy");

            return View();
        }

        private IActionResult CreateJsonResult(int totalRecords, object obj)
        {
            return Json(new
            {
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalRecords,
                aaData = obj
            });
        }
        public async Task<IActionResult> ManualReport(DataTableParamViewModel param)
        {
            object obj = null;
            int totalRecords = 0;

            if (!string.IsNullOrEmpty(param.target))
            {
                string[] orderMapping = null;
                if (string.IsNullOrEmpty(param.orderType))
                    param.orderType = "asc";
                if (param.startDate == null)
                    param.startDate = DateTime.Now;

                switch (param.target)
                {
                    case "report":
                        orderMapping = new string[] { "ReportDate", "SalesCode", "SalesName", "CustomerCode", "CustomerName" };
                        totalRecords = await reportService.GetManualReportCount(param.search, param.startDate.Value);

                        if (totalRecords == 0)
                            return CreateJsonResult((int)totalRecords, new List<ViewModels.DailyReportItemViewModel>());

                        var dbResult = await reportService.GetManualReport(param.search, param.startDate.Value, param.length, param.start, orderMapping[param.orderCol], param.orderType);

                        obj = dbResult;
                        break;
                }
            }

            return Json(new
            {
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalRecords,
                aaData = obj
            });
        }

        public IActionResult Index()
        {
            ViewBag.SelectedMenu = "produktivitas";
            ViewBag.CurrentDate = DateTime.UtcNow.ToLocalTime().ToString("MM/dd/yyyy");

            return View();
        }

        public IActionResult Driver()
        {
            ViewBag.SelectedMenu = "driver";
            ViewBag.CurrentDate = DateTime.UtcNow.ToLocalTime().ToString("MM/dd/yyyy");

            return View();
        }
        
        private double GetDistanceFromRoute(LatLng[] routes)
        {
            if (routes == null || routes.Length == 0)
                return 0;

            double distance = 0;
            int i = 0;
            MapsHelper mapsHelper = new MapsHelper();
            while (i < routes.Count())
            {
                if (i + 1 < routes.Count())
                    distance += mapsHelper.Distance(routes[i], routes[i + 1]);
                i++;
            }

            return distance/1000;
        }

        public async Task<IActionResult> NewDetail(string id, DateTime date)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            CmsDetailUserReportViewModel model = new CmsDetailUserReportViewModel();

            try
            {
                model = await assignmentReportService.GetDetail(id, date);
                if (model == null)
                    return NotFound();

                var wayRoute =  reportService.GetUserLocationActivity(id, date);
                
                model.TotalKM = GetDistanceFromRoute(wayRoute);
                model.LocationHistoryJson = string.Format("{0}", JsonConvert.SerializeObject(model.LocationHistory));
                model.VisitHistoryJson = string.Format("{0}", JsonConvert.SerializeObject(model.VisitHistory));
                model.UserWayRoutesJson = string.Format("{0}", JsonConvert.SerializeObject(wayRoute));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            ViewBag.GoogleMapsKey = applicationSetting.GoogleMapsKey;
            return View(model);
        }

        public async Task<IActionResult> Details(string id, DateTime date)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            CmsDetailUserReportViewModel model = new CmsDetailUserReportViewModel();

            try
            {
                model = await assignmentReportService.GetDetail(id, date);
                if (model == null)
                    return NotFound();

                var wayRoute = reportService.GetUserLocationActivity(id, date);

                model.TotalKM = GetDistanceFromRoute(wayRoute);
                model.LocationHistoryJson = string.Format("{0}", JsonConvert.SerializeObject(model.LocationHistory));
                model.VisitHistoryJson = string.Format("{0}", JsonConvert.SerializeObject(model.VisitHistory));
                model.UserWayRoutesJson = string.Format("{0}", JsonConvert.SerializeObject(wayRoute));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            ViewBag.GoogleMapsKey = applicationSetting.GoogleMapsKey;
            return View(model);
        }
    }
}