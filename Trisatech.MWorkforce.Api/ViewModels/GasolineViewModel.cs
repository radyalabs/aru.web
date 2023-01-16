using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Api.ViewModels
{
    public class GasolineViewModel
    {
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("remarks")]
        public string Remarks { get; set; }
        [JsonProperty("liter")]
        public double Liter { get; set; }
        public IFormFile Attachment { get; set; }
        public IFormFile Attachment1 { get; set; }
        public IFormFile Attachment2 { get; set; }
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
        [JsonProperty("event_date")]
        public DateTime EventDate { get; set; }
    }
}
