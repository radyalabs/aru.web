using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Trisatech.MWorkforce.Business.Entities
{
    public class SurveyModel
    {
        [JsonProperty("survey_id")]
        public string SurveyId { get; set; }
        [JsonProperty("name")]
        public string SurveyName { get; set; }
        [JsonProperty("link")]
        public string SurveyLink { get; set; }
        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }
        [JsonProperty("end_date")]
        public DateTime EndDate { get; set; }
    }
}
