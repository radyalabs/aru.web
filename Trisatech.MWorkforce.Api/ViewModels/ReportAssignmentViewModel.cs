using Newtonsoft.Json;
using Trisatech.AspNet.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Api.ViewModels
{
    public class ReportAssignmentViewModel
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }
        [JsonProperty("user_code")]
        public string UserCode { get; set; }
        [JsonProperty("user_name")]
        public string UserName { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("start_time_utc")]
        public DateTime? StartTime { get; set; }
        
        [JsonProperty("start_time")]
        public DateTime? StartedTime
        {
            get
            {
                return StartTime == null ? StartTime : StartTime.Value.Add7Hour();
            }
        }

        [JsonProperty("end_time_utc")]
        public DateTime? EndedTime { get; set; }
        
        [JsonProperty("end_time")]
        public DateTime? EndTime { get
            {
                return (EndedTime == null ? EndedTime : EndedTime.Value.Add7Hour());
            }
        }

        [JsonProperty("total_task_completed")]
        public int TotalTaskCompleted { get; set; }

        [JsonProperty("total_task_in_progress")]
        public int TotalTaskInProgress { get; set; }
        [JsonProperty("total_task_failed")]
        public int TotalTaskFailed { get; set; }
        [JsonProperty("total_task")]
        public int TotalTask {
            get {
                return (ListVisit != null ? ListVisit.Count : 0);
            }
        }

        [JsonProperty("total_invoice")]
        public decimal TotalInvoice
        {
            get
            {
                return (ListVisit != null ? ListVisit.Sum(x => x.InvoiceAmount) : 0);
            }
        }

        [JsonProperty("total_payment")]
        public decimal TotalPayment
        {
            get
            {
                return (ListVisit != null ? ListVisit.Sum(x => x.PaymentAmount) : 0);
            }
        }

        [JsonProperty("total_work_time")]
        public double TotalWorkTime
        {
            get
            {
                double result = 0;

                if (StartTime != null && EndedTime != null)
                {
                    result = Math.Round((EndedTime - StartTime).Value.TotalHours, 2);
                }
                
                return result;
            }
        }

        [JsonProperty("lost_time")]
        public int TotalLossTime { get; set; }

        [JsonProperty("total_loss_time")]
        public double LostTime
        {
            get
            {
                return Math.Round((double) TotalLossTime / 3600, 2);
            }
        }

        [JsonProperty("total_time_at_store")]
        public double TotalTimeAtStore {
            get
            {
                return (from a in ListVisit
                        where a.EndTime != null && !(a.StartTime.ToString("yyyy/MM/dd").Equals("0001/01/01"))
                        select a.EndTime.Value.Subtract(a.StartTime).TotalHours
                        ).Sum();
            }
        }

        public List<VisitDetail> ListVisit { get; set; }
    }
    public class VisitDetail
    {
        public string AssignmentId { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime AssignmentDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }

}
