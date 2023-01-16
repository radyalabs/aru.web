using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Trisatech.MWorkforce.Api.Helpers;
using Trisatech.MWorkforce.Api.Model;
using Trisatech.MWorkforce.Api.ViewModels;
using Trisatech.MWorkforce.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Trisatech.AspNet.Common.Extensions;

namespace Trisatech.MWorkforce.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService accountService;
        private readonly ApplicationSetting applicationSetting;
        private Trisatech.AspNet.Common.Models.JsonEntity json;
        public AuthController(IAccountService accountService, IOptions<ApplicationSetting> options)
        {
            this.accountService = accountService;
            applicationSetting = options.Value;
            json = new Trisatech.AspNet.Common.Models.JsonEntity();
        }
        
        [HttpPost]
        public IActionResult Post([FromBody]LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    loginViewModel.Password = loginViewModel.Password.ToSHA256();
                    
                    var result = accountService.Login(loginViewModel.Username, loginViewModel.Password);

                    var tokenLogin = ApiAuthHelper.Set(result, true, applicationSetting.EncryptionKey, applicationSetting.VerificationKey);

                    //update to db with new Token
                    string shaToken = tokenLogin.ToSHA256();

                    accountService.SetSession(result.UserId, shaToken);

                    var userProfile = new UserProfileViewModel
                    {
                        UserCode = result.UserCode,
                        RoleCode = result.RoleCode,
                        RoleName = result.RoleName,
                        UserName = result.Name
                    };

                    json.SetError(false);
                    json.AddAlert((int)HttpStatusCode.OK, "Success");
                    json.AddData(new { token = tokenLogin, profile = userProfile });

                }
                catch (ApplicationException appEx)
                {
                    json.SetError(false);
                    json.AddAlert((int)HttpStatusCode.NotAcceptable, appEx.Message);
                    json.AddData(null);
                }
                catch(Exception ex)
                {
                    json.SetError(true);
                    json.AddAlert((int)HttpStatusCode.InternalServerError, ex.Message);
                    json.AddData(null);
                }
            }
            else
            {
                json.SetError(false);
                json.AddAlert((int)HttpStatusCode.NotAcceptable, "");
            }
            
            return Ok(json);
        }
        
        [HttpPost]
        [Route("logout")]
        public IActionResult Logout([FromHeader(Name = "X-Aru-Token")] string token)
        {
            try
            {
                accountService.Logout(token.ToSHA256());

                json.SetError(false);
                json.AddAlert((int)HttpStatusCode.OK, "Success");
                json.AddData(null);
            }
            catch(Exception ex)
            {
                json.SetError(true);
                json.AddAlert((int)HttpStatusCode.Unauthorized, ex.Message);
                json.AddData(null);
            }

            return Ok(json);
        }
    }
}