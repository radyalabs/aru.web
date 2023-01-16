using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trisatech.MWorkforce.Business.Entities;
using Trisatech.MWorkforce.Business.Interfaces;
using Trisatech.MWorkforce.Cms.Helpers;
using Trisatech.MWorkforce.Cms.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trisatech.AspNet.Common.Extensions;
using Trisatech.AspNet.Common.Helpers;
using Trisatech.MWorkforce.Domain;

namespace Trisatech.MWorkforce.Cms.Controllers
{
    [Authorize]
    public class AjaxDataTableController : Controller
    {
        private readonly IAssignmentService assignmentService;
        private readonly IAssignmentReportService assignmentReportService;
        private readonly IUserManagementService userManagementService;
        private readonly ICustomerService customerService;
        private readonly ISurveyService surveyService;
        private readonly IProductService productService;
        private readonly IContentManagementService newsService;
		private readonly ITerritoryService territoryService;

        public AjaxDataTableController(IAssignmentService assignmentService, 
            IUserManagementService userManagementService, 
            ICustomerService customerService, 
            ISurveyService surveyService,
            IProductService productService, 
            IContentManagementService newsService, 
            ITerritoryService territoryService,
            IAssignmentReportService assignmentReportService)
        {
            this.assignmentService = assignmentService;
            this.userManagementService = userManagementService;
            this.customerService = customerService;
            this.surveyService = surveyService;
            this.productService = productService;
            this.newsService = newsService;
			this.territoryService = territoryService;
            this.assignmentReportService = assignmentReportService;
        }

        private string[] GetTerritoryByRoleCode(UserAuthenticated userAuthenticated)
        {
            string[] territory = Array.Empty<string>();

            if (userAuthenticated.RoleCode == "SA")
            {
                var territoryResult = territoryService.Get();
                if (territoryResult != null && territoryResult.Any())
                    territory = territoryResult.Select(x => x.TerritoryId).ToArray();
            }
            else
            {
                territory = userAuthenticated.TerritoryId.ToArray();
            }

            return territory;
        }

        [Authorize(Roles = "SA,OPR,SPV")]
        public async Task<IActionResult> Report(DataTableParamViewModel param)
        {
            object obj = null;
            long totalRecords = 0;

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
                        orderMapping = new string[] { "UserCode", "UserName", "StartTime", "EndTime" };

                        string baseUrl = SiteHelper.GetBaseUrl(this.Request);
                        List<ReportDataTableViewModel> dataList = new List<ReportDataTableViewModel>();

                        totalRecords = await assignmentService.GetListAssignmentReportCount(param.search, param.startDate.Value, orderMapping[param.orderCol], param.orderType);

                        if (totalRecords == 0)
                            return CreateJsonResult((int)totalRecords, dataList);

                        var dbResult = await assignmentService.GetListAssignmentReport(param.search, param.startDate.Value, param.length, param.start, orderMapping[param.orderCol], param.orderType);
                        if (dbResult != null && dbResult.Count > 0)
                        {
                            foreach (var item in dbResult)
                            {
                                ReportDataTableViewModel newItem = new ReportDataTableViewModel();
                                CopyProperty.CopyPropertiesTo(item, newItem);
                                
                                newItem.BaseUrl = baseUrl;

                                dataList.Add(newItem);
                            }
                        }
                        
                        obj = dataList;
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

        [Authorize(Roles = "SA,OPR,SPV")]
        public async Task<IActionResult> DriverReport(DataTableParamViewModel param)
        {
            var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);
            string[] regions = GetTerritoryByRoleCode(userAuth);

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
                        try
                        {
                            orderMapping = new string[] { "UserCode", "UserName", "StartTime", "EndTime" };

                            string baseUrl = SiteHelper.GetBaseUrl(this.Request);

                            List<CmsReportAssignmentViewModel> dataList = new List<CmsReportAssignmentViewModel>();
                            totalRecords = await assignmentReportService.GetAssingmentReportLength(
                                regions,
                                param.search, param.startDate.Value, "DRIVER");

                            if (totalRecords == 0)
                                return CreateJsonResult(totalRecords, dataList);

                            var dbResult = await assignmentReportService.GetListAssignmentReport(
                                regions,
                                param.search, 
                                param.startDate.Value, 
                                param.length, 
                                param.start, 
                                orderMapping[param.orderCol], 
                                param.orderType, "DRIVER");
                            if (dbResult != null && dbResult.Count > 0)
                            {
                                foreach (var item in dbResult)
                                {
                                    item.BaseUrl = baseUrl;
                                }

                                dataList = dbResult;
                            }
                            obj = dataList;
                        }
                        catch (Exception ex)
                        {
                            totalRecords = 0;
                            obj = new List<CmsReportAssignmentViewModel>();
                        }

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

        [Authorize(Roles = "SA,OPR,SPV")]
        public async Task<IActionResult> NewReport(DataTableParamViewModel param)
        {
            var userAuth = AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);
            string[] regions = GetTerritoryByRoleCode(userAuth);

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
                        try
                        {
                            orderMapping = new string[] { "UserCode", "UserName", "StartTime", "EndTime" };

                            string baseUrl = SiteHelper.GetBaseUrl(this.Request);

                            List<CmsReportAssignmentViewModel> dataList = new List<CmsReportAssignmentViewModel>();
                            totalRecords = await assignmentReportService.GetAssingmentReportLength(regions, param.search, param.startDate.Value);

                            if (totalRecords == 0)
                                return CreateJsonResult(totalRecords, dataList);

                            var dbResult = await assignmentReportService.GetListAssignmentReport(regions, param.search, param.startDate.Value, param.length, param.start, orderMapping[param.orderCol], param.orderType);
                            if(dbResult != null && dbResult.Count > 0)
                            {
                                foreach(var item in dbResult)
                                {
                                    item.BaseUrl = baseUrl;
                                }

                                dataList = dbResult;
                            }
                            obj = dataList;
                        }
                        catch(Exception ex)
                        {
                            totalRecords = 0;
                            obj = new List<CmsReportAssignmentViewModel>();
                        }

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
        [Authorize(Roles = "SA,OPR")]
        public IActionResult Customers(DataTableParamViewModel param)
        {
            object obj = null;
            int totalRecords = 0;
            if (string.IsNullOrEmpty(param.orderType))
                param.orderType = "asc";
            if (!string.IsNullOrEmpty(param.target))
            {
                string[] orderMapping = null;

                switch (param.target)
                {
                    case "customer":
                        orderMapping = new string[] { "CreatedDt", "CustomerName", "CustomerCode", "CustomerPhoneNumber", "CustomerAddress" };
                        string baseUrl = SiteHelper.GetBaseUrl(this.Request);
                        var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);
                        List<CustomerDataTableViewModel> dataList = new List<CustomerDataTableViewModel>();

                        if (userAuth.RoleCode == "SA")
                        {
                            totalRecords = customerService.TotalCustomers(param.search, param.length, param.start, orderMapping[param.orderCol], param.orderType);

                            if (totalRecords == 0)
                                return CreateJsonResult(totalRecords, dataList);

                            var dbResult = customerService.GetCustomers(param.search, param.length, param.start, orderMapping[param.orderCol], param.orderType, param.startDate, param.endDate);
                            if (dbResult != null)
                            {
                                foreach (var item in dbResult)
                                {
                                    CustomerDataTableViewModel newItem = new CustomerDataTableViewModel();
                                    CopyProperty.CopyPropertiesTo(item, newItem);
                                    newItem.BaseUrl = baseUrl;

                                    dataList.Add(newItem);
                                }
                            }
                        } else
                        {
                            totalRecords = customerService.TotalCustomers(param.search, param.length, param.start, orderMapping[param.orderCol], param.orderType, userAuth);

                            if (totalRecords == 0)
                                return CreateJsonResult(totalRecords, obj);

                            var dbResult = customerService.GetCustomers(param.search, param.length, param.start, orderMapping[param.orderCol], param.orderType, param.startDate, param.endDate, userAuth);
                            if (dbResult != null)
                            {
                                foreach (var item in dbResult)
                                {
                                    CustomerDataTableViewModel newItem = new CustomerDataTableViewModel();
                                    CopyProperty.CopyPropertiesTo(item, newItem);
                                    newItem.BaseUrl = baseUrl;

                                    dataList.Add(newItem);
                                }
                            }
                        }
                        obj = dataList;
                        break;
                }
            }
            
            return CreateJsonResult(totalRecords, obj);
        }
        [Authorize(Roles = "SA,OPR")]
        public IActionResult Outlets(DataTableParamViewModel param)
        {
            object obj = null;
            int totalRecords = 0;
            if (string.IsNullOrEmpty(param.orderType))
                param.orderType = "asc";
            if (!string.IsNullOrEmpty(param.target))
            {
                string[] orderMapping = null;

                switch (param.target)
                {
                    case "customer":
                        orderMapping = new string[] { "CreatedDt", "CustomerName", "CustomerCode", "CustomerPhoneNumber", "CustomerAddress" };
                        string baseUrl = SiteHelper.GetBaseUrl(this.Request);
                        var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);

                        List<CustomerDataTableViewModel> dataList = new List<CustomerDataTableViewModel>();

                        if (userAuth.RoleCode == "SA")
                        {
                            totalRecords = customerService.TotalOutlet(param.search, param.length, param.start, orderMapping[param.orderCol], param.orderType);

                            if (totalRecords == 0)
                                return CreateJsonResult(totalRecords, dataList);

                            var dbResult = customerService.GetOutlets(param.search, param.length, param.start, orderMapping[param.orderCol], param.orderType, param.startDate, param.endDate);
                            if (dbResult != null)
                            {
                                foreach (var item in dbResult)
                                {
                                    CustomerDataTableViewModel newItem = new CustomerDataTableViewModel();
                                    CopyProperty.CopyPropertiesTo(item, newItem);
                                    
                                    newItem.BaseUrl = baseUrl;

                                    dataList.Add(newItem);
                                }
                            }
                        }
                        else
                        {
                            totalRecords = customerService.TotalOutlet(param.search, param.length, param.start, orderMapping[param.orderCol], param.orderType, userAuth);

                            if (totalRecords == 0)
                                return CreateJsonResult(totalRecords, dataList);

                            var dbResult = customerService.GetOutlets(param.search, param.length, param.start, orderMapping[param.orderCol], param.orderType, param.startDate, param.endDate, userAuth);
                            if (dbResult != null)
                            {
                                foreach (var item in dbResult)
                                {
                                    CustomerDataTableViewModel newItem = new CustomerDataTableViewModel();
                                    CopyProperty.CopyPropertiesTo(item, newItem);
                                    newItem.BaseUrl = baseUrl;

                                    dataList.Add(newItem);
                                }
                            }
                        }
                        obj = dataList;
                        break;
                }
            }
    
            return CreateJsonResult(totalRecords, obj);
        }
        [Authorize(Roles = "SA,OPR")]
        public IActionResult Contacts(DataTableParamViewModel param)
        {
            var userAuth = AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);
            string[] regions = GetTerritoryByRoleCode(userAuth);

            object obj = null;
            int totalRecords = 0;
            if (string.IsNullOrEmpty(param.orderType))
                param.orderType = "asc";
            if (!string.IsNullOrEmpty(param.target))
            {
                string[] orderMapping = null;

                switch (param.target)
                {
                    case "contact":
                        orderMapping = new string[] { "CreatedDt", "ContactName", "Position", "ContactNumber", "Email" };

                        string baseUrl = SiteHelper.GetBaseUrl(this.Request);
                        List<ContactDataTableViewModel> dataList = new List<ContactDataTableViewModel>();

                        totalRecords = customerService.TotalContact(
                            regions,
                            param.search,
                            param.length, param.start, orderMapping[param.orderCol], param.orderType);

                        if (totalRecords == 0)
                            return CreateJsonResult(totalRecords, dataList);

                        var dbResult = customerService.GetWebContact(
                            regions,
                            param.search, param.length, param.start, orderMapping[param.orderCol], param.orderType);
                        if (dbResult != null)
                        {
                            foreach (var item in dbResult)
                            {
                                ContactDataTableViewModel newItem = new ContactDataTableViewModel();
                                CopyProperty.CopyPropertiesTo(item, newItem);
                                newItem.BaseUrl = baseUrl;

                                dataList.Add(newItem);
                            }
                        }

                        obj = dataList;
                        break;
                }
            }

            return CreateJsonResult(totalRecords, obj);
        }
        [Authorize(Roles = "SA,OPR,SALES")]
        public IActionResult Assignment(DataTableParamViewModel param)
        {
            object obj = null;
            int totalRecords = 0;
           
            if (!string.IsNullOrEmpty(param.target))
            {
                string[] orderMapping = null;
                DateTime? startDate = (param.startDate == null ? DateTime.UtcNow : param.startDate);
                DateTime? endDate = (param.endDate == null ? DateTime.UtcNow : param.endDate);

                if (string.IsNullOrEmpty(param.orderType))
                    param.orderType = "asc";
                switch (param.target)
                {
                    case "assignment":
                        orderMapping = new string[] { "CreatedDt", "AssignmentName", "AgentCode", "AgentName", "AssignmentAddress", "AssigmentStatus" };
                        string baseUrl = SiteHelper.GetBaseUrl(this.Request);
                        List<AssignmentDataTableViewModel> dataList = new List<AssignmentDataTableViewModel>();

						var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);
						
						if (userAuth.RoleCode == "SA")
						{
							totalRecords = (int)assignmentService.GetListAssignmentCount(param.search, orderMapping[param.orderCol], param.orderType, param.status, startDate, endDate);

                            if (totalRecords == 0)
                                return CreateJsonResult(totalRecords, dataList);

                            var dbResult = assignmentService.GetListAssignment(param.search, param.length, param.start, orderMapping[param.orderCol], param.orderType, startDate, endDate, param.status);

                            dataList = CreateAssignmentViewModelData(dbResult, baseUrl);
                        }else
						{
							totalRecords = (int)assignmentService.GetListAssignmentCount(param.search, orderMapping[param.orderCol], param.orderType, param.status, startDate, endDate);
                            if (totalRecords == 0)
                                return CreateJsonResult(totalRecords, dataList);

                            var dbResult = assignmentService.GetListAssignment(param.search, param.length, param.start, orderMapping[param.orderCol], param.orderType, startDate, endDate, param.status, userAuth);

                            dataList = CreateAssignmentViewModelData(dbResult, baseUrl);
						}
                        
                        obj = dataList;
                        break;
                }
            }

            return CreateJsonResult(totalRecords, obj);
        }
        private List<AssignmentDataTableViewModel> CreateAssignmentViewModelData(List<AssignmentModel> dbResult, string baseUrl)
        {
            List<AssignmentDataTableViewModel> result = new List<AssignmentDataTableViewModel>();

            if (dbResult != null && dbResult.Count > 0)
            {
                foreach (var item in dbResult)
                {
                    AssignmentDataTableViewModel newItem = new AssignmentDataTableViewModel();
                    CopyProperty.CopyPropertiesTo(item, newItem);

                    newItem.AssignmentDate = item.AssignmentDate;
                    newItem.BaseUrl = baseUrl;

                    result.Add(newItem);
                }
            }

            return result;
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
        [Authorize(Roles = "SA,OPR,SALES")]
        public IActionResult DriverAssignment(DataTableParamViewModel param)
        {
            object obj = null;
            int totalRecords = 0;

            if (!string.IsNullOrEmpty(param.target))
            {
                string[] orderMapping = null;

                if (string.IsNullOrEmpty(param.orderType))
                    param.orderType = "asc";
                switch (param.target)
                {
                    case "assignment":
                        orderMapping = new string[] { "CreatedDt", "AssignmentName", "AgentCode", "AgentName", "AssignmentAddress", "AssigmentStatus" };

                        string baseUrl = SiteHelper.GetBaseUrl(this.Request);
                        List<AssignmentDataTableViewModel> dataList = new List<AssignmentDataTableViewModel>();

                        var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);

                        if (userAuth.RoleCode == "SA")
                        {
                            totalRecords = (int)assignmentService.GetListAssignmentCount(param.search, orderMapping[param.orderCol], param.orderType, param.status, param.startDate, param.endDate, "DRIVER");

                            if (totalRecords == 0)
                                return CreateJsonResult(totalRecords, dataList);

                            var dbResult = assignmentService.GetListAssignment(param.search, param.length, param.start, orderMapping[param.orderCol], param.orderType, param.startDate, param.endDate, param.status, null, "DRIVER");

                            dataList = CreateAssignmentViewModelData(dbResult, baseUrl);

                        }
                        else
                        {
                            totalRecords = (int)assignmentService.GetListAssignmentCount(param.search, orderMapping[param.orderCol], param.orderType, param.status, param.startDate, param.endDate, "DRIVER");

                            if (totalRecords == 0)
                                return CreateJsonResult(totalRecords, dataList);

                            var dbResult = assignmentService.GetListAssignment(param.search, param.length, param.start, orderMapping[param.orderCol], param.orderType, param.startDate, param.endDate, param.status, userAuth, "DRIVER");

                            dataList = CreateAssignmentViewModelData(dbResult, baseUrl);
                        }

                        obj = dataList;
                        break;
                }
            }
            
            return CreateJsonResult(totalRecords, obj);
        }
        [Authorize(Roles = "SA")]
        public IActionResult Users(DataTableParamViewModel param)
        {
            object obj = null;
            int totalRecords = 0;
            if (string.IsNullOrEmpty(param.orderType))
                param.orderType = "asc";
            if (!string.IsNullOrEmpty(param.target))
            {
                string[] orderMapping = null;

                switch (param.target)
                {
                    case "agent":
                        orderMapping = new string[] { "UserName", "UserCode", "Account.Role.RoleName", "UserEmail", "UserPhone" };
                        string baseUrl = SiteHelper.GetBaseUrl(this.Request);
                        List<UserDataTableViewModel> dataList = new List<UserDataTableViewModel>();

                        totalRecords = (int)userManagementService.TotalUsers(param.search, param.length, param.start, orderMapping[param.orderCol], param.orderType);

                        if (totalRecords == 0)
                            return CreateJsonResult(totalRecords, dataList);

                        var dbResult = userManagementService.Users(param.search, param.length, param.start, orderMapping[param.orderCol], param.orderType);
                        if(dbResult != null)
                        {
                            foreach(var item in dbResult)
                            {
                                UserDataTableViewModel newItem = new UserDataTableViewModel();
                                CopyProperty.CopyPropertiesTo(item, newItem);
                                newItem.BaseUrl = baseUrl;

                                dataList.Add(newItem);
                            }
                        }

                        obj = dataList;
                        break;
                }
            }
    
            return CreateJsonResult(totalRecords, obj);
        }
        [Authorize(Roles = "SA")]
        public IActionResult Surveys(DataTableParamViewModel param)
        {
            object obj = null;
            int totalRecords = 0;
            if (string.IsNullOrEmpty(param.orderType))
                param.orderType = "asc";
            if (!string.IsNullOrEmpty(param.target))
            {
                string[] orderMapping = null;
                
                switch (param.target)
                {
                    case "survey":
                        orderMapping = new string[] { "CreatedDt", "SurveyName", "StartDate", "EndDate"};
                        string baseUrl = SiteHelper.GetBaseUrl(this.Request);
                        List<SurveyDataTableViewModel> dataList = new List<SurveyDataTableViewModel>();

                        totalRecords = surveyService.TotalRow(param.search, param.length, param.start, orderMapping[param.orderCol], param.orderType);

                        if (totalRecords == 0)
                            return CreateJsonResult(totalRecords, dataList);

                        var dbResult = surveyService.Get(param.search, param.length, param.start, orderMapping[param.orderCol], param.orderType);
                        if (dbResult != null)
                        {
                            foreach (var item in dbResult)
                            {
                                SurveyDataTableViewModel newItem = new SurveyDataTableViewModel();
                                CopyProperty.CopyPropertiesTo(item, newItem);

                                newItem.StartDate = newItem.StartDate.ToUtcID();
                                newItem.EndDate = newItem.EndDate.ToUtcID();
                                newItem.BaseUrl = baseUrl;

                                dataList.Add(newItem);
                            }
                        }

                        obj = dataList;
                        break;
                }
            }
            
             return CreateJsonResult(totalRecords, obj);
        }
        [Authorize(Roles = "SA")]
        public IActionResult News(DataTableParamViewModel param)
        {
            object obj = null;
            int totalRecords = 0;
            if (string.IsNullOrEmpty(param.orderType))
                param.orderType = "asc";
            if (!string.IsNullOrEmpty(param.target))
            {
                string[] orderMapping = null;

                switch (param.target)
                {
                    case "news":
                        orderMapping = new string[] { "CreatedDt", "Title", "PublishedDate" };

                        string baseUrl = SiteHelper.GetBaseUrl(this.Request);
                        List<NewsDataTableViewModel> dataList = new List<NewsDataTableViewModel>();

                        totalRecords = newsService.TotalNews(param.search, param.length, param.start, orderMapping[param.orderCol], param.orderType);

                        if (totalRecords == 0)
                            return CreateJsonResult(totalRecords, dataList);

                        var dbResult = newsService.Get(param.search, param.length, param.start, orderMapping[param.orderCol], param.orderType);
                        if (dbResult != null)
                        {
                            foreach (var item in dbResult)
                            {
                                NewsDataTableViewModel newItem = new NewsDataTableViewModel();
                                CopyProperty.CopyPropertiesTo(item, newItem);

                                newItem.PublishedDate = item.PublishedDate.ToUtcID();
                                newItem.BaseUrl = baseUrl;

                                dataList.Add(newItem);
                            }
                        }

                        obj = dataList;
                        break;
                }
            }

            return CreateJsonResult(totalRecords, obj);
        }
        [Authorize(Roles = "SA")]
        public IActionResult Products(DataTableParamViewModel param)
        {
            object obj = null;
            int totalRecords = 0;
            if (string.IsNullOrEmpty(param.orderType))
                param.orderType = "asc";
            if (!string.IsNullOrEmpty(param.target))
            {
                string[] orderMapping = null;
                
                switch (param.target)
                {
                    case "product":
                        orderMapping = new string[] { "CreatedDt", "ProductCode", "ProductName", "ProductModel", "ProductPrice" };
                        string baseUrl = SiteHelper.GetBaseUrl(this.Request);
                        List<ProductDataTableViewModel> dataList = new List<ProductDataTableViewModel>();

                        totalRecords = productService.TotalRow(param.search, param.length, param.start, orderMapping[param.orderCol], param.orderType);

                        if (totalRecords == 0)
                            return CreateJsonResult(totalRecords, dataList);

                        var dbResult = productService.Get(param.search, param.length, param.start, orderMapping[param.orderCol], param.orderType);
                        if (dbResult != null)
                        {
                            foreach (var item in dbResult)
                            {
                                ProductDataTableViewModel newItem = new ProductDataTableViewModel();
                                CopyProperty.CopyPropertiesTo(item, newItem);
                                newItem.BaseUrl = baseUrl;

                                dataList.Add(newItem);
                            }
                        }

                        obj = dataList;
                        break;
                }
            }
            
            return CreateJsonResult(totalRecords, obj);
        }
		[Authorize(Roles = "SA")]
		public IActionResult Territory(DataTableParamViewModel param)
		{
			object obj = null;
			int totalRecords = 0;
			if (string.IsNullOrEmpty(param.orderType))
				param.orderType = "asc";
			if (!string.IsNullOrEmpty(param.target))
			{
				string[] orderMapping = null;

				switch (param.target)
				{
					case "Territory":
						orderMapping = new string[] { "CreatedDt", "TerritoryId", "Name", "Desc" };
						string baseUrl = SiteHelper.GetBaseUrl(this.Request);
						List<TerritoryDataTableViewModel> dataList = new List<TerritoryDataTableViewModel>();

						totalRecords = territoryService.Total(param.search, param.length, param.start, orderMapping[param.orderCol], param.orderType);

                        if (totalRecords == 0)
                            return CreateJsonResult(totalRecords, dataList);

                        var dbResult = territoryService.Get(param.search, param.length, param.start);
						if (dbResult != null)
						{
							foreach (var item in dbResult)
							{
								TerritoryDataTableViewModel newItem = new TerritoryDataTableViewModel();
								CopyProperty.CopyPropertiesTo(item, newItem);
								newItem.BaseUrl = baseUrl;

								dataList.Add(newItem);
							}
						}

						obj = dataList;
						break;
				}
			}
    
            return CreateJsonResult(totalRecords, obj);
        }
	}
}