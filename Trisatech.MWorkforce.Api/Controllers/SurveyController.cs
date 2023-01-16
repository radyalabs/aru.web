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

namespace Trisatech.MWorkforce.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/survey")]
    public class SurveyController : AppBaseController
    {
        private readonly IAccountService accountService;
        private readonly ISurveyService surveyService;
        private Trisatech.AspNet.Common.Models.JsonEntity json;

        public SurveyController(
            IAccountService accountService,
            ISurveyService surveyService,
            IOptions<ApplicationSetting> options):base(options)
        {
            this.accountService = accountService;
            this.surveyService = surveyService;
            json = new Trisatech.AspNet.Common.Models.JsonEntity();
        }
        
        [HttpPost]
        public IActionResult Post([FromHeader(Name = "X-Aru-Token")]string authKey, AnswerSurveyViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    if (UserAuth == null)
                        return Ok(UnAuthorizeResponse.UnauthorizeResponse());

                    AnswerModel surveyAnswer = new AnswerModel();
                    surveyAnswer.SurveyId = model.SurveyId;
                    surveyAnswer.AssignmentCode = model.AssignmentCode;
                    surveyAnswer.AssignmentId = model.AssignmentId;
                    surveyAnswer.UserId = UserAuth.UserId;
                    surveyAnswer.AnswerDate = model.CreatedDate;

                    surveyService.Answer(surveyAnswer, UserAuth.UserId);

                    json.SetError(false);
                    json.AddAlert((int)HttpStatusCode.OK, "Success");
                }
                catch(Exception ex)
                {
                    json.SetError(true);
                    json.AddAlert((int)HttpStatusCode.InternalServerError, ex.Message);
                }
            }
            else
            {
                json.SetError(false);
                json.AddAlert((int)HttpStatusCode.NotAcceptable, "Not acceptable");
                json.AddData(ModelState);
            }

            return Ok(json);
        }
    }
}