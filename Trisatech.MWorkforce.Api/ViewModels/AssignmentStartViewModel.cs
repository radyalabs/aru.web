using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Api.ViewModels
{
    public class AssignmentStartViewModel
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("start_time")]
        public DateTime? StartDate { get; set; }
        [JsonProperty("remarks")]
        public string Remarks { get; set; }
    }
}
