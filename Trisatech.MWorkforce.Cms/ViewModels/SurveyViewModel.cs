using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Cms.ViewModels
{
    public class SurveyViewModel
    {
        [JsonProperty("survey_id")]
        public string SurveyId { get; set; }
        [Display(Name = "Name")]
        [JsonProperty("name")]
        [Required]
        public string SurveyName { get; set; }
        [Display(Name = "Google Form Link")]
        [JsonProperty("link")]
        [Required]
        public string SurveyLink { get; set; }
        [Display(Name = "Start date")]
        [JsonProperty("start_date")]
        [Required]
        public DateTime StartDate { get; set; }
        [Display(Name = "End date")]
        [JsonProperty("end_date")]
        [Required]
        public DateTime EndDate { get; set; }
    }
}
