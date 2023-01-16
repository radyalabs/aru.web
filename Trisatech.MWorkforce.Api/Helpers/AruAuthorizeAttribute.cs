using Trisatech.MWorkforce.Api.Model;
using Trisatech.MWorkforce.Business.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Api.Helpers
{
    public class AruAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public ApplicationSetting ApplicationSetting;
        public string AuthHeaderKey;
        public string EncryptionKey;
        public string VerificationKey;
        
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                StringValues tokenValue;
                context.HttpContext.Request.Headers.TryGetValue(AuthHeaderKey, out tokenValue);

                var result = ApiAuthHelper.Get<UserAuthenticated>((string)tokenValue, EncryptionKey, VerificationKey);
                if(result == null)
                {
                    //access denied
                }
            }
            catch(Exception ex)
            {
                //access denied
            }
        }
    }
}

