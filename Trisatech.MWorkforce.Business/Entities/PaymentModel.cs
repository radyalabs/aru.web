using Trisatech.MWorkforce.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Trisatech.MWorkforce.Business.Entities
{
    public class PaymentModel
    {
        [JsonProperty("payment_id")]
        public string PaymentId { get; set; }
        [JsonProperty("assignment_id")]
        public string AssignmentId { get; set; }
        [JsonProperty("invoice_code")]
        public string InvoiceCode { get; set; }
        [JsonProperty("invoice_amount")]
        public decimal InvoiceAmount { get; set; }
        [JsonProperty("payment_amount")]
        public decimal PaymentAmount { get; set; }
        [JsonProperty("payment_debt")]
        public decimal PaymentDebt { get; set; }
        [JsonProperty("payment_channel")]
        public PaymentChannel PaymentChannel { get; set; }
        [JsonProperty("transfer_bank")]
        public string TransferBank { get; set; }
        [JsonProperty("transfer_amount")]
        public decimal TransferAmount { get; set; }
        [JsonProperty("transfer_date")]
        public DateTime? TransferDate { get; set; }
        [JsonProperty("giro_number")]
        public string GiroNumber { get; set; }
        [JsonProperty("giro_due_date")]
        public DateTime? GiroDueDate { get; set; }
        [JsonProperty("giro_photo")]
        public string GiroPhoto { get; set; }
        [JsonProperty("giro_blob_name")]
        public string BlobName { get; set; }
        [JsonProperty("giro_amount")]
        public decimal GiroAmount { get; set; }
        [JsonProperty("cash_amount")]
        public decimal CashAmount { get; set; }
    }
}
