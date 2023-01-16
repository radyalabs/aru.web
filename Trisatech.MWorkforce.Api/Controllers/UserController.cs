using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Trisatech.MWorkforce.Api.Helpers;
using Trisatech.MWorkforce.Api.Model;
using Trisatech.MWorkforce.Api.ViewModels;
using Trisatech.MWorkforce.Business.Entities;
using Trisatech.MWorkforce.Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Trisatech.AspNet.Common.Helpers;
using Trisatech.MWorkforce.Domain;

namespace Trisatech.MWorkforce.Api.Controllers
{   
    [Produces("application/json")]
    [Route("api/user")]
    public class UserController : AppBaseController
    {
        private readonly IAccountService accountService;
        private readonly IUserActivityService activityService;
        private Trisatech.AspNet.Common.Models.JsonEntity json;
        public UserController(
            IAccountService accountService,
            IUserActivityService activityService,
            IOptions<ApplicationSetting> options):base(options)
        {
            this.accountService = accountService;
            this.activityService = activityService;
            json = new Trisatech.AspNet.Common.Models.JsonEntity();
        }

        [HttpGet]
        [Route("agent")]
        [ProducesResponseType(typeof(IEnumerable<UserAgentModel>), 200)]
        public IActionResult Agent([FromHeader(Name = "X-Aru-Token")] string authKey, string role = AppConstant.Role.SALES)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SetUserAuth();
                    if (UserAuth == null)
                        return Ok(UnAuthorizeResponse.UnauthorizeResponse());


                    var listAgent = accountService.GetUserAgent(role, UserAuth.TerritoryId);

                    json.SetError(false);
                    json.AddAlert((int)HttpStatusCode.OK, "Success");
                    json.AddData(listAgent);
                }
                catch (Exception ex)
                {
                    json.SetError(false);
                    json.AddAlert((int)HttpStatusCode.InternalServerError, ex.Message);
                    json.AddData(null);
                }
            }
            else
            {
                var messageError = ErrorHelper.Error(ModelState);

                json.SetError(false);
                json.AddAlert((int)HttpStatusCode.NotAcceptable, "");
                json.AddData(messageError);
            }

            return Ok(json);
        }

        [HttpPost]
        [Route("activity")]
        public IActionResult LocationUpdate([FromHeader(Name = "X-Aru-Token")] string authKey, [FromBody]UpdateLocationViewModel locationViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SetUserAuth();

                    if (UserAuth == null)
                        return Ok(UnAuthorizeResponse.UnauthorizeResponse());

                    if(locationViewModel.CreatedDt == null)
                    {
                        locationViewModel.CreatedDt = DateTime.UtcNow;
                    }
                    else
                    {
                        locationViewModel.CreatedDt = locationViewModel.CreatedDt.Value.ToUniversalTime();
                    }

                    UserActivityModel activityModel = new UserActivityModel();
                    CopyProperty.CopyPropertiesTo(locationViewModel, activityModel);
                    activityModel.CreatedBy = UserAuth.UserId;
                    

                    activityService.Add(activityModel);
                    json = new Trisatech.AspNet.Common.Models.JsonEntity()
                    {
                        Error = false,
                        Data = null,
                        Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.OK, Message = "Success" }
                    };

                }
                catch (Exception ex)
                {
                    json = new Trisatech.AspNet.Common.Models.JsonEntity()
                    {
                        Error = true,
                        Data = null,
                        Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.InternalServerError, Message = ex.Message }
                    };
                }
            }
            else
            {
                json = new Trisatech.AspNet.Common.Models.JsonEntity()
                {
                    Error = false,
                    Data = ErrorHelper.Error(ModelState),
                    Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.InternalServerError, Message = "NotAcceptable" }
                };
            }

            return Ok(json);
        }
    }
}