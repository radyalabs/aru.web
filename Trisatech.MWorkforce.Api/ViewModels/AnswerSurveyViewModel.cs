using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Api.ViewModels
{
    public class AnswerSurveyViewModel
    {
        [JsonProperty("assignment_code")]
        public string AssignmentCode { get; set; }
        [JsonProperty("assigment_id")]
        public string AssignmentId { get; set; }
        [JsonRequired]
        [JsonProperty("created_date")]
        public DateTime CreatedDate { get; set; }
        [JsonRequired]
        [JsonProperty("user_id")]
        public string UserId { get; set; }
        [JsonRequired]
        [JsonProperty("survey_id")]
        public string SurveyId { get; set; }
    }
}
