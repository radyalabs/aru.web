using Trisatech.MWorkforce.Business.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Api.ViewModels
{
    public class DashboardViewModel
    { 
        [JsonProperty("total_task")]
        public int TotalTask { get; set; }
        [JsonProperty("failed")]
        public int Failed { get; set; }
        [JsonProperty("success")]
        public int Success { get; set; }
        [JsonProperty("on_going")]
        public int OnGoing { get; set; }
        [JsonProperty("assignment_report")]
        public IEnumerable<ReportAssignmentViewModel> AssignmentReport { get; set; }
        [JsonProperty("total_work_time")]
        public double TotalWorkTime { get; set; }
        [JsonProperty("total_lost_time")]
        public double TotalLostTime { get; set; }
    }
}
