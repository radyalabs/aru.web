using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Trisatech.MWorkforce.Api.Helpers;
using Trisatech.MWorkforce.Api.Model;
using Trisatech.MWorkforce.Business.Entities;

namespace Trisatech.MWorkforce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppBaseController : ControllerBase
    {
        protected UserAuthenticated UserAuth;
        protected ApplicationSetting AppSetting;
        public AppBaseController(IOptions<ApplicationSetting> options)
        {
            AppSetting = options.Value;
        }
        protected void SetUserAuth()
        {
            UserAuth = ApiAuthHelper.Get<UserAuthenticated>(HttpContext.Request.Headers["X-Aru-Token"].ToString(), AppSetting.EncryptionKey, AppSetting.VerificationKey);
        }
    }
}
