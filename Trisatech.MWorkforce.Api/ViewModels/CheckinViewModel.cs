using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Api.ViewModels
{
    public enum CheckInMode { ONLINE, OFFLINE}
    public class CheckinViewModel
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
        [JsonProperty("start_time")]
        public DateTime StartTime { get; set; }
        [JsonProperty("user_id")]
        [Required]
        public string UserId { get; set; }
        [JsonProperty("comment")]
        public string Comment { get; set; }
        [JsonProperty("mode")]
        public CheckInMode Mode { get; set; }
        [JsonProperty("distance")]
        public double Distance { get; set; }
        public IFormFile Attachment { get; set; }
    }
}
