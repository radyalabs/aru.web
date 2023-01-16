using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Trisatech.MWorkforce.Api.Helpers;
using Trisatech.MWorkforce.Api.Interfaces;
using Trisatech.MWorkforce.Api.Model;
using Trisatech.MWorkforce.Business.Entities;
using Trisatech.MWorkforce.Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Trisatech.AspNet.Common.Extensions;

namespace Trisatech.MWorkforce.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/dashborad")]
    public class DashboradController : AppBaseController
    {
        private Trisatech.AspNet.Common.Models.JsonEntity json;
        private readonly IUserReportService reportService;
        private readonly IAccountService accountService;
        
        public DashboradController(IOptions<ApplicationSetting> options, IUserReportService userReportService, IAccountService accountService)
            :base(options)
        {
            json = new Trisatech.AspNet.Common.Models.JsonEntity();
            reportService = userReportService;
            this.accountService = accountService;
        }

        [HttpGet]
        public IActionResult Get([FromHeader(Name = "X-Aru-Token")] string authKey, DateTime date)
        {
            SetUserAuth();
            if (UserAuth == null)
                return Ok(UnAuthorizeResponse.UnauthorizeResponse());

            try
            {
                string partnerId = string.Empty;
                if (UserAuth.RoleCode == Domain.AppConstant.Role.SUPERVISOR)
                    partnerId = accountService.GetCheckinPartner(UserAuth.UserId, DateTime.UtcNow.Date);

                var result = reportService.GetDashbord(UserAuth.UserId, date.Date.ToUtc());
                if (result != null)
                {
                    var reportAssignment = reportService.GetListAssignmentReport(new List<string> { UserAuth.UserId, partnerId }, date.Date.ToUtc());
                    if(reportAssignment!=null && reportAssignment.Count > 0)
                    {
                        var currentUserReport = reportAssignment.FirstOrDefault(x => x.UserId.Equals(UserAuth.UserId));
                        if (currentUserReport != null)
                        {
                            result.TotalWorkTime = currentUserReport.TotalWorkTime == 0 ? Math.Round(DateTime.UtcNow.Subtract(currentUserReport.StartTime.Value).TotalHours, 2) : currentUserReport.TotalWorkTime;
                            result.TotalLostTime = currentUserReport.TotalLossTime;

                            reportAssignment.Remove(currentUserReport);
                        }
                    }

                    result.AssignmentReport = reportAssignment;
                }

                json.SetError(false);
                json.AddAlert((int)HttpStatusCode.OK, "Success");
                json.AddData(result);
            }catch(Exception ex)
            {
                json.SetError(true);
                json.AddAlert((int)HttpStatusCode.InternalServerError, ex.ToString());
                json.AddData(null);
            }

            return Ok(json);
        }
    }
}