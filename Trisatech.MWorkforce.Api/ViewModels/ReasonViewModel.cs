using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Api.ViewModels
{
    public class ReasonViewModel
    {
        [JsonProperty("reason_code")]
        public string Code { get; set; }
        [JsonProperty("reason_name")]
        public string Name { get; set; }
    }
}
