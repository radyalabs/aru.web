using Newtonsoft.Json;
using Trisatech.AspNet.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Trisatech.MWorkforce.Business.Entities
{
    public static class ReportConstant
    {
        public const string AGENT_ARRIVED = "AGENT_ARRIVED";
        public const string AGENT_STARTED = "AGENT_STARTED";
        public const string TASK_COMPLETED = "TASK_COMPLETED";
        public const string TASK_FAILED = "TASK_FAILED";
        public const string TASK_RECEIVED = "TASK_RECEIVED";

        public const string COMPLETEUNVERIFIED = "Complete/Non Verif";
        public const string COMPLETEVERIFIED = "Complete/Verif";
        public const string UNCOMPLETEUNVERIFIED = "Uncomplete/Verif";
        public const string UNCOMPLETEVERIFIED = "Uncomplete/Non Verif";
    }
    public class ReportAssignmentModel
    {
        public string UserId { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public int TotalLostTime { get; set; }
        public string Name { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndedTime { get; set; }
        public int TotalTaskCompleted { get; set; }
        public int TotalTaskInProgress { get; set; }
        public int TotalTaskFailed { get; set; }
        public int TotalTask { get; set; }
        public int TotalWorkTime { get; set; }
        public int TotalTaskVerified { get; set; }
    }

    public class CmsReportAssignmentViewModel
    {
        [JsonProperty("first_checkin_time")]
        public string FirstCheckin { get
            {
                return (FirstCheckinTime == null || FirstCheckinTime.Value.ToString("yyyy/MM/dd").Equals("0001/01/01") ? "-" 
                    : FirstCheckinTime.Value.Add7Hour().ToString("dd-MM-yyyy HH:mm:ss"));
            }
        }

        public DateTime? FirstCheckinTime { get; set; }
        [JsonProperty("cms_total_invoice")]
        public decimal CmsTotalTagihan { get; set; }
        public decimal CmsTotalPayment { get; set; }
        [JsonProperty("cms_total_payment")]
        public string CmsJsonTotalPayment {
            get {
                return CmsTotalPayment.ToString("C", new CultureInfo("id-ID"));
                }
        }
        [JsonProperty("cms_total_kunjungan")]
        public int CmsTotalKunjungan { get; set; }
        [JsonProperty("base_url")]
        public string BaseUrl { get; set; }
        [JsonProperty("url_detail")]
        public string UrlDetail
        {
            get
            {
                return BaseUrl + "/report/details/" + UserId + "?date=" + StartTime.Value.ToUtcID().ToString("MM-dd-yyyy");
            }
        }
        [JsonProperty("attendance_attach_url")]
        public string AttendanceAttachmentUrl { get; set; }
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
        public string StartedTime
        {
            get
            {
                return StartTime == null ? "-" : StartTime.Value.Add7Hour().ToString("dd-MM-yyyy HH:mm:ss");
            }
        }

        [JsonProperty("end_time_utc")]
        public DateTime? EndedTime { get; set; }

        [JsonProperty("end_time")]
        public string EndTime
        {
            get
            {
                return (EndedTime == null ? "-" : EndedTime.Value.Add7Hour().ToString("dd-MM-yyyy HH:mm:ss"));
            }
        }

        [JsonProperty("total_task_completed")]
        public int TotalTaskCompleted { get; set; }

        [JsonProperty("total_task_in_progress")]
        public int TotalTaskInProgress { get; set; }
        [JsonProperty("total_task_failed")]
        public int TotalTaskFailed { get; set; }
        [JsonProperty("total_task")]
        public int TotalTask
        {
            get
            {
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
                return Math.Round((double)TotalLossTime / 3600, 2);
            }
        }

        [JsonProperty("loss_time_at_first_checkin")]
        public double TotalTimeAtFirstCheckin { get
            {
                return Math.Round((double)LostTimeAtFirstCheckin / 3600, 2);
            } 
        }

        public double LostTimeAtFirstCheckin { get; set; }
        [JsonProperty("total_time_at_store")]
        public double TotalTimeAtStore
        {
            get
            {
                if (ListVisit == null)
                    return 0;
                double sum = (from a in ListVisit
                        where a.EndTime != null && !(a.StartTime.ToString("yyyy/MM/dd").Equals("0001/01/01"))
                        select a.EndTime.Value.Subtract(a.StartTime).TotalHours
                        ).Sum();

                return Math.Round(sum, 2);
            }
        }
        [JsonProperty("total_verified_task")]
        public int TotalVerifiedTask { get; set; }
        public List<CmsVisitDetail> ListVisit { get; set; }
    }

    public class CmsVisitDetail
    {
        public string AssignmentId { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime AssignmentDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
    public class CmsDetailUserReportViewModel
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
        public string StartedTime
        {
            get
            {
                return StartTime.Add7Hour().ToString("dd-MM-yyyy HH:mm:ss");
            }
        }

        [JsonProperty("end_time")]
        public string EndTime
        {
            get
            {
                return (EndedTime == null ? "-" : EndedTime.Value.Add7Hour().ToString("dd-MM-yyyy HH:mm:ss"));
            }
        }

        [JsonProperty("total_task")]
        public int TotalTask
        {
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

        [JsonProperty("total_payment")]
        public decimal TotalPayment
        {
            get
            {
                if (VisitHistory == null)
                    return 0;
                return (VisitHistory.Sum(x => x.PaymentAmount));
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
                if (VisitHistory == null || VisitHistory.Count == 0)
                    return 0;

                double sum = (from a in VisitHistory
                        where a.EndedTime != null && a.StartedTime != null
                        && !(a.StartedTime.Value.ToString("yyyy/MM/dd").Equals("0001/01/01"))
                        select a.EndedTime.Value.Subtract(a.StartedTime.Value).TotalHours
                        ).Sum();


                return Math.Round(sum, 2);
            }
        }

        [JsonProperty("visit_history")]
        public List<CmsVisitHistoryViewModel> VisitHistory { get; set; }
        [JsonProperty("location_history")]
        public List<CmsLocationHistoryViewModel> LocationHistory { get; set; }
        public string LocationHistoryJson { get; set; }
        public string VisitHistoryJson { get; set; }
        public string UserWayRoutesJson { get; set; }
        [JsonProperty("checkin_image")]
        public string CheckInImage { get; set; }
        [JsonProperty("checkout_image")]
        public string CheckoutImage { get; set; }
    }

    public partial class CmsLocationHistoryViewModel
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
            get; set;
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
            get; set;
        }

        [JsonProperty("loss_time")]
        public double LossTime
        {
            get
            {
                return SalesTimeResult - GoogleTimeResult;
            }
        }
    }

    public partial class CmsVisitHistoryViewModel
    {
        [JsonProperty("customer_code")]
        public string CustomerCode { get; set; }
        [JsonProperty("customer_name")]
        public string CustomerName { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("task_name")]
        public string TaskName { get; set; }
        [JsonProperty("reason")]
        public string Reason { get; set; }

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
        public DateTime? EndTime
        {
            get
            {
                return EndedTime == null ? EndedTime : EndedTime.Value.Add7Hour();
            }
        }

        [JsonProperty("status")]
        public string Status
        {
            get
            {
                if (ReportConstant.TASK_COMPLETED == VisitStatus && IsVerified)
                    return ReportConstant.COMPLETEVERIFIED;
                else if (ReportConstant.TASK_COMPLETED == VisitStatus && !IsVerified)
                    return ReportConstant.COMPLETEUNVERIFIED;
                else if (ReportConstant.TASK_COMPLETED != VisitStatus && IsVerified)
                    return ReportConstant.UNCOMPLETEVERIFIED;
                else
                    return ReportConstant.UNCOMPLETEUNVERIFIED;
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
        public double LossTime
        {
            get
            {
                return (SalesTime - GoogleTime) < 0 ? 0 : (SalesTime - GoogleTime)/3600;
            }
        }


        [JsonProperty("sales_time")]
        public double SalesTimeResult
        {
            get
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
        [JsonProperty("attachment")]
        public string Attachment { get; set; }
    }

}
