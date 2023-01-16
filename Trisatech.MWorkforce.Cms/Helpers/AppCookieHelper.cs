using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using Trisatech.AspNet.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Cms.Helpers
{
    public class AppCookieHelper
    {
        public static async void Set(object obj, string role, bool isRemember, string key, Microsoft.AspNetCore.Http.HttpContext context)
        {
            string identifier = key + "|" + DateTime.Now.ToString("yyyyMMddHHmmssffff");

            string strJsonUser = JsonConvert.SerializeObject(obj);

            var userClaimIdentity = new UserClaimIdentity("MSalesForce", "MSalesForceIdentity");

            var ident = new ClaimsIdentity(userClaimIdentity,
                    new[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, identifier),
                    new Claim(ClaimTypes.UserData, strJsonUser),
                    new Claim(ClaimTypes.Authentication, identifier),
                    new Claim(ClaimTypes.Role, role),
                    });

            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(ident),
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddDays(14)
                    });
        }

        public static T Get<T>(Microsoft.AspNetCore.Http.HttpContext context) where T : class
        {
            T userAuth = null;
            
            if (!(context.User != null &&
                context.User.Identity != null /*&&
                httpContext.User.Identity.IsAuthenticated*/))
            {
                return null;
            }

            var userData = ((ClaimsIdentity)context.User.Identity).FindFirst(ClaimTypes.UserData);

            userAuth = JsonConvert.DeserializeObject<T>(userData.Value);

            return userAuth;
        }

        public async static void LogOut(Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
