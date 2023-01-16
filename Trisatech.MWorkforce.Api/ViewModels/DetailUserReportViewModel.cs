using Trisatech.MWorkforce.Api.Helpers;
using Newtonsoft.Json;
using Trisatech.AspNet.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trisatech.MWorkforce.Domain;

namespace Trisatech.MWorkforce.Api.ViewModels
{
    public class DetailUserReportViewModel
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }
        [JsonProperty("user_code")]
        public string UserCode { get; set; }
        [JsonProperty("role_name")]
        public string RoleName { get; set; }
        [JsonProperty("user_name")]
        public string UserName { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("user_email")]
        public string UserEmail { get; set; }
        [JsonProperty("user_phone")]
        public string UserPhone { get; set; }
        [JsonProperty("total_km")]
        public double TotalKM { get; set; }
        [JsonProperty("start_latitude")]
        public double StartLatitude { get; set; }
        [JsonProperty("end_latitude")]
        public double EndLatitude { get; set; }
        [JsonProperty("start_longitude")]
        public double StartLongitude { get; set; }
        [JsonProperty("end_longitude")]
        public double EndLongitude { get; set; }

        [JsonProperty("start_time_utc")]
        public DateTime StartTime { get; set; }

        [JsonProperty("end_time_utc")]
        public DateTime? EndedTime { get; set; }


        [JsonProperty("start_time")]
        public DateTime StartedTime { get
            {
                return StartTime.Add7Hour();
            }
        }

        [JsonProperty("end_time")]
        public DateTime? EndTime { get
            {
                return EndedTime == null ? EndedTime : EndedTime.Value.Add7Hour();
            }
        }

        [JsonProperty("total_task")]
        public int TotalTask {
            get
            {
                return VisitHistory.Count;
            }
        }

        [JsonProperty("total_task_completed")]
        public int TotalTaskCompleted { get; set; }

        [JsonProperty("total_work_time")]
        public double TotalWorkTime
        {
            get
            {
                double result = 0;

                if (EndedTime != null && !StartTime.ToString("yyyy/MM/dd").Equals("0001/01/01"))
                {
                    result = Math.Round((EndedTime - StartTime).Value.TotalHours, 2);
                }

                return result;
            }
        }

        [JsonProperty("lost_time")]
        public int TotalLossTime { get; set; }

        [JsonProperty("total_loss_time")]
        public int LostTime
        {
            get
            {
                return TotalLossTime / 3600;
            }
        }

        [JsonProperty("total_time_at_store")]
        public double TotalTimeAtStore
        {
            get
            {
                return (from a in VisitHistory
                        where a.EndedTime != null && a.StartedTime != null 
                        && !(a.StartedTime.Value.ToString("yyyy/MM/dd").Equals("0001/01/01"))
                        select a.EndedTime.Value.Subtract(a.StartedTime.Value).TotalHours
                        ).Sum();
            }
        }

        [JsonProperty("visit_history")]
        public List<VisitHistoryViewModel> VisitHistory { get; set; }
        [JsonProperty("location_history")]
        public List<LocationHistoryViewModel> LocationHistory { get; set; }
    }

    public partial class LocationHistoryViewModel
    {
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("latitude")]
        public double? Latitude { get; set; }
        [JsonProperty("longitude")]
        public double? Longitude { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("is_active")]
        public bool IsActive
        {
            get
            {
                if (StartTime == null)
                {
                    return false;
                }
                else if (StartTime.Value.Add7Hour().Date == DateTime.UtcNow.Add7Hour().Date)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public DateTime? StartTime { get; set; }

        [JsonProperty("checkin_date")]
        public string CheckInDateTime
        {
            get
            {
                if (StartTime == null)
                {
                    return "Never Checkin";
                }
                else
                {
                    return StartTime.Value.Add7Hour().ToString("dd/MM/yyyy HH:mm:ss");
                }
            }
        }

        [JsonProperty("google_time")]
        public double GoogleTimeResult
        {
            get
            {
                return GoogleTime / 3600;
            }
        }
        public double GoogleTime
        {
            get;set;
        }

        [JsonProperty("sales_time")]
        public double SalesTimeResult
        {
            get
            {
                return SalesTime / 3600;
            }
        }

        public double SalesTime
        {
            get;set;
        }

        [JsonProperty("loss_time")]
        public double LossTime { get
            {
                return SalesTimeResult - GoogleTimeResult;
            }
        }
    }

    public partial class VisitHistoryViewModel
    {
        [JsonProperty("customer_name")]
        public string CustomerName { get; set; }
        [JsonProperty("task_name")]
        public string TaskName { get; set; }

        [JsonProperty("started_time_utc")]
        public DateTime? StartedTime { get; set; }

        [JsonProperty("ended_time_utc")]
        public DateTime? EndedTime { get; set; }

        [JsonProperty("started_time")]
        public DateTime? StartTime
        {
            get
            {
                if (!StartedTime.Value.ToString("yyyy/MM/dd").Equals("0001/01/01"))
                    return StartedTime.Value.Add7Hour();
                else
                    return EndedTime?.Add7Hour();
            }
        }

        [JsonProperty("ended_time")]
        public DateTime? EndTime { get
            {
                return EndedTime == null ? EndedTime : EndedTime.Value.Add7Hour();
            }
        }

        [JsonProperty("status")]
        public string Status
        {
            get
            {
                if (AppConstant.AssignmentStatus.TASK_COMPLETED == VisitStatus && IsVerified)
                    return AppConstant.AssignmentStatus.COMPLETEVERIFIED;
                else if (AppConstant.AssignmentStatus.TASK_COMPLETED == VisitStatus && !IsVerified)
                    return AppConstant.AssignmentStatus.COMPLETEUNVERIFIED;
                else if (AppConstant.AssignmentStatus.TASK_COMPLETED != VisitStatus && !IsVerified)
                    return AppConstant.AssignmentStatus.UNCOMPLETEUNVERIFIED;
                else
                    return AppConstant.AssignmentStatus.UNCOMPLETEVERIFIED;
            }
        }

        public string VisitStatus { get; set; }

        [JsonProperty("total_invoice")]
        public int TotalInvoice { get; set; }
        [JsonProperty("invoice_amount")]
        public decimal InvoiceAmount { get; set; }
        [JsonProperty("payment_amount")]
        public decimal PaymentAmount { get; set; }
        [JsonProperty("is_verified")]
        public bool IsVerified { get; set; }

        [JsonProperty("loss_time")]
        public double LossTime {
            get
            {
                return GoogleTime / 3600;
            }
        }
        

        [JsonProperty("sales_time")]
        public double SalesTimeResult { get
            {
                return SalesTime / 3600;
            }
        }
        public double SalesTime { get; set; }
        
        public double GoogleTime { get; set; }

        [JsonProperty("google_time")]
        public double GoogleTimeResult
        {
            get
            {
                return GoogleTime / 3600;
            }
        }
    }
}
