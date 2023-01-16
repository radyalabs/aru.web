using Trisatech.MWorkforce.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Api.ViewModels
{
    public class UpdateLocationViewModel
    {
        [Required]
        [JsonRequired]
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        [Required]
        [JsonRequired]
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
        [Required]
        [JsonRequired]
        [JsonProperty("type")]
        public UserActivityEnum ActivityTypeEnum { get; set; }
        [JsonProperty("date")]
        public DateTime? CreatedDt { get; set; }
        [JsonProperty("user_id")]
        public string CreatedBy { get; set; }
        [JsonProperty("assignment_id")]
        public string AssignmentId { get; set; }
        [JsonProperty("status_code")]
        public string AssignmentStatusCode { get; set; }
    }
}
