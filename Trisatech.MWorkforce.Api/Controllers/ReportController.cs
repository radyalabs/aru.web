using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Trisatech.MWorkforce.Api.Helpers;
using Trisatech.MWorkforce.Api.Interfaces;
using Trisatech.MWorkforce.Api.Model;
using Trisatech.MWorkforce.Api.ViewModels;
using Trisatech.MWorkforce.Business.Entities;
using Trisatech.MWorkforce.Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Trisatech.MWorkforce.Domain;
using Trisatech.AspNet.Common.Extensions;

namespace Trisatech.MWorkforce.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Report")]
    public class ReportController : AppBaseController
    {
        private Trisatech.AspNet.Common.Models.JsonEntity json;
        private readonly IUserReportService reportService;
        private readonly IAccountService accountService;

        public ReportController(IOptions<ApplicationSetting> options, IUserReportService userReportService, IAccountService accountService):base(options)
        {
            reportService = userReportService;
            this.accountService = accountService;
            json = new Trisatech.AspNet.Common.Models.JsonEntity();
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ReportAssignmentViewModel>), 200)]
        public IActionResult Get([FromHeader(Name = "X-Aru-Token")] string authKey, DateTime date, int limit = 10, int offset = 0)
        {
            SetUserAuth();

            if (UserAuth == null)
                return Ok(UnAuthorizeResponse.UnauthorizeResponse());

            try
            {
                if (UserAuth.RoleCode != AppConstant.Role.SUPERVISOR)
                {
                    json.SetError(false);
                    json.AddAlert((int)HttpStatusCode.NotAcceptable, "Method not allowed");
                    json.AddData(null);

                    return Ok(json);
                }
                List<string> users = new List<string> { UserAuth.UserId };

                var agent = accountService.GetUserAgent(AppConstant.Role.SALES, UserAuth.TerritoryId.Distinct().ToList());
                if (agent != null)
                    users.AddRange(agent.Select(x => x.UserId).ToList());

                var reportAssignment = reportService.GetListAssignmentReport(users, date.Date.ToUtc(), limit, offset);
                
                json.SetError(false);
                json.AddAlert((int)HttpStatusCode.OK, "Success");
                json.AddData(reportAssignment);

            }catch(ApplicationException appEx)
            {
                json.SetError(false);
                json.AddAlert((int)HttpStatusCode.NotAcceptable, appEx.ToString());
                json.AddData(null);
            }
            catch (Exception ex)
            {
                json.SetError(true);
                json.AddAlert((int)HttpStatusCode.InternalServerError, ex.ToString());
                json.AddData(null);
            }

            return Ok(json);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DetailUserReportViewModel), 200)]
        public IActionResult Get([FromHeader(Name = "X-Aru-Token")] string authKey, [FromRoute]string id, DateTime date)
        {
            SetUserAuth();

            if (UserAuth == null)
                return Ok(UnAuthorizeResponse.UnauthorizeResponse());

            try
            {
                if (UserAuth.RoleCode != AppConstant.Role.SUPERVISOR)
                {
                    json.SetError(false);
                    json.AddAlert((int)HttpStatusCode.NotAcceptable, "Method not allowed");
                    json.AddData(null);

                    return Ok(json);
                }

                var result = reportService.GetDetailAssignmentReport(id, date.Date.ToUtc());

                json.SetError(false);
                json.AddAlert((int)HttpStatusCode.OK, "Success");
                json.AddData(result);
            }
            catch (ApplicationException appEx)
            {
                json.SetError(false);
                json.AddAlert((int)HttpStatusCode.NotAcceptable, appEx.ToString());
                json.AddData(null);
            }
            catch (Exception ex)
            {
                json.SetError(true);
                json.AddAlert((int)HttpStatusCode.InternalServerError, ex.ToString());
                json.AddData(null);
            }

            return Ok(json);
        }
    }
}