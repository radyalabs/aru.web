using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Api.ViewModels
{
    public class UserProfileViewModel
    {
        [JsonProperty("user_name")]
        public string UserName { get; set; }
        [JsonProperty("user_code")]
        public string UserCode { get; set; }
        [JsonProperty("role_code")]
        public string RoleCode { get; set; }
        [JsonProperty("role_name")]
        public string RoleName { get; set; }
    }
}
