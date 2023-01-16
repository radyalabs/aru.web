using Trisatech.MWorkforce.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Trisatech.MWorkforce.Business.Entities
{
    public class AssignmentModel
    {
        [JsonProperty("assignment_id")]
        public string AssignmentId { get; set; }
        [JsonProperty("assignment_code")]
        public string AssignmentCode { get; set; }
        [JsonProperty("assignment_name")]
        public string AssignmentName { get; set; }
        [JsonProperty("assignment_date")]
        public DateTime AssignmentDate { get; set; }
        [JsonProperty("assignment_address")]
        public string AssignmentAddress { get; set; }
        [JsonProperty("assignment_status_code")]
        public string AssignmentStatusCode { get; set; }
        [JsonProperty("assignment_longitude")]
        public double? Longitude { get; set; }
        [JsonProperty("assignment_latitude")]
        public double? Latitude { get; set; }
        [JsonProperty("assignment_start_time")]
        public DateTime StartTime { get; set; }
        [JsonProperty("assignment_end_time")]
        public DateTime? EndTime { get; set; }
        [JsonProperty("lost_time")]
        public double LostTime { get; set; }
        [JsonProperty("remarks")]
        public string Remarks { get; set; }
        [JsonProperty("assignment_type")]
        public AssignmentType AssignmentType { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("contact")]
        public ContactModel Contact { get; set; }
        [JsonProperty("agent_id")]
        public string AgentId { get; set; }
        [JsonProperty("agent_code")]
        public string AgentCode { get; set; }
        [JsonProperty("agent_name")]
        public string AgentName { get; set; }
        [JsonProperty("contact_name")]
        public string ContactName { get; set; }
        [JsonProperty("contact_number")]
        public string ContactNumber { get; set; }

        [JsonProperty("is_assignment_detail_completed")]
        public bool IsAssignmentDetailCompleted { get; set; }
        [JsonProperty("attendance_attach_url")]
        public string AttendanceAttachUrl { get; set; }

        [JsonProperty("invoices")]
        public List<InvoiceModel> Invoices { get; set; }
        [JsonProperty("orders")]
        public List<OrderModel> Orders { get; set; }
        [JsonProperty("payments")]
        public List<PaymentModel> Payments { get; set; }
        [JsonProperty("survey")]
        public List<SurveyModel> Survey { get; set; }
		[JsonProperty("User")]
		public List<UserModel> Users { get; set; }
	}
}
