using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Trisatech.MWorkforce.Business.Entities
{
    public class InvoiceModel
    {
        [JsonProperty("invoice_id")]
        public string InvoiceId { get; set; }
        [JsonProperty("invoice_code")]
        public string InvoiceCode { get; set; }
        [JsonProperty("assignment_code")]
        public string AssignmentCode { get; set; }
        [JsonProperty("due_date")]
        public DateTime? DueDate { get; set; }
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("type")]
        public short Type { get; set; }
    }
}
