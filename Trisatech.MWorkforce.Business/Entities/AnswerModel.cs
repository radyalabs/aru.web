using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Trisatech.MWorkforce.Business.Entities
{
    public class AnswerModel
    {
        [JsonProperty("answer_id")]
        public int AnswerId { get; set; }
        [JsonProperty("assignment_code")]
        public string AssignmentCode { get; set; }
        [JsonProperty("assigment_id")]
        public string AssignmentId { get; set; }
        [JsonProperty("user_id")]
        public string UserId { get; set; }
        [JsonProperty("survey_id")]
        public string SurveyId { get; set; }

        public string SurveyName { get; set; }
        public string SurveyLink { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AgentName { get; set; }
        public string AgentRole { get; set; }
        public string AgentRoleName { get; set; }
        public string AssignmentTitle { get; set; }
        public DateTime AnswerDate { get; set; }
    }
}
