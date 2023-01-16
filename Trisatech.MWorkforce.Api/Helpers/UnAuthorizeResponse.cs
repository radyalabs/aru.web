using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Api.Helpers
{
    public class UnAuthorizeResponse
    {
        public static Trisatech.AspNet.Common.Models.JsonEntity UnauthorizeResponse()
        {
            var json = new Trisatech.AspNet.Common.Models.JsonEntity()
            {
                Error = false,
                Data = null,
                Alerts = new Trisatech.AspNet.Common.Models.JsonAlert { Code = (int)HttpStatusCode.Unauthorized, Message = "Unauthorized" }
            };

            return json;
        }
    }
}
