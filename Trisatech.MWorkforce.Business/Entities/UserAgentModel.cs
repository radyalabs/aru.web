using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Trisatech.MWorkforce.Business.Entities
{
    public class UserAgentModel
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }
        [JsonProperty("user_code")]
        public string UserCode { get; set; }
        [JsonProperty("user_name")]
        public string UserName { get; set; }
        [JsonProperty("user_phone")]
        public string UserPhone { get; set; }
    }
}
