using Trisatech.MWorkforce.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Trisatech.MWorkforce.Business.Entities
{
    public class UserActivityModel
    {
        [JsonProperty("user_activity_id")]
        public long UserActivityId { get; set; }
        [JsonProperty("assignment_id")]
        public string AssignmentId { get; set; }
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
        [JsonProperty("activity_type")]
        public UserActivityEnum ActivityTypeEnum { get; set; }
        [JsonProperty("assignment_status_code")]
        public string AssignmentStatusCode { get; set; }
        [JsonProperty("date_time")]
        public DateTime CreatedDt { get; set; }
        public string CreatedBy { get; set; }
        [JsonProperty("assignment")]
        public AssignmentModel Assignment { get; set; }
        [JsonProperty("user")]
        public UserModel User { get; set; }
    }
}
