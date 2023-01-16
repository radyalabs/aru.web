using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Cms.Helpers
{
    public class SiteHelper
    {
        public static string GetBaseUrl(HttpRequest context)
        {
            string scheme = context.Headers["X-Forwarded-Proto"];
            if(string.IsNullOrEmpty(scheme))
            {
                scheme = context.Scheme;
            }
            return $"{scheme}://{context.Host}{context.PathBase}";
        }

        public static string GetCurrentUrl(HttpRequest context)
        {            
            string scheme = context.Headers["X-Forwarded-Proto"];
            if(string.IsNullOrEmpty(scheme))
            {
                scheme = context.Scheme;
            }
            return $"{scheme}://{context.Host}{context.PathBase}{context.Path}";
        }
    }
}
