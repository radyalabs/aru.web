using Trisatech.MWorkforce.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Api.ViewModels
{
    public class PaymentViewModel
    {
        [JsonProperty("payment_id")]
        public string PaymentId { get; set; }
        [Required]
        [JsonProperty("assignment_id")]
        public string AssignmentId { get; set; }
        [Required]
        [JsonProperty("invoice_code")]
        public string InvoiceCode { get; set; }
        [Required]
        [JsonProperty("invoice_amount")]
        public decimal InvoiceAmount { get; set; }
        [Required]
        [JsonProperty("payment_amount")]
        public decimal PaymentAmount { get; set; }
        [JsonProperty("payment_debt")]
        public decimal PaymentDebt { get; set; }
        [JsonProperty("payment_channel")]
        public PaymentChannel PaymentChannel { get; set; }
        [JsonProperty("cash_amount")]
        public decimal CashAmount { get; set; }
        //[Required]
        [JsonProperty("otp")]
        public string Otp { get; set; }

        #region transfer
        //TRANSFER 1
        [JsonProperty("transfer_bank")]
        public string TransferBank { get; set; }
        [JsonProperty("transfer_amount")]
        public decimal TransferAmount { get; set; }
        [JsonProperty("transfer_date")]
        public DateTime? TransferDate { get; set; }

        //TRANSFER 2
        [JsonProperty("transfer_bank")]
        public string TransferBank1 { get; set; }
        [JsonProperty("transfer_amount")]
        public decimal? TransferAmount1 { get; set; }
        [JsonProperty("transfer_date")]
        public DateTime? TransferDate1 { get; set; }

        //TRANSFER 3
        [JsonProperty("transfer_bank")]
        public string TransferBank2 { get; set; }
        [JsonProperty("transfer_amount")]
        public decimal? TransferAmount2 { get; set; }
        [JsonProperty("transfer_date")]
        public DateTime? TransferDate2 { get; set; }

        //TRANSFER 4
        [JsonProperty("transfer_bank")]
        public string TransferBank3 { get; set; }
        [JsonProperty("transfer_amount")]
        public decimal? TransferAmount3 { get; set; }
        [JsonProperty("transfer_date")]
        public DateTime? TransferDate3 { get; set; }

        //TRANSFER 5
        [JsonProperty("transfer_bank")]
        public string TransferBank4 { get; set; }
        [JsonProperty("transfer_amount")]
        public decimal? TransferAmount4 { get; set; }
        [JsonProperty("transfer_date")]
        public DateTime? TransferDate4 { get; set; }
        #endregion

        #region GIRO
        //GIRO 1
        [JsonProperty("giro_number_1")]
        public string GiroNumber { get; set; }
        [JsonProperty("giro_photo_1")]
        public string GiroPhoto { get; set; }
        [JsonProperty("giro_amount_1")]
        public decimal GiroAmount { get; set; }
        [JsonProperty("giro_due_date_1")]
        public DateTime? GiroDueDate { get; set; }

        //GIRO 2
        [JsonProperty("giro_number_2")]
        public string GiroNumber1 { get; set; }
        [JsonProperty("giro_photo_2")]
        public string GiroPhoto1 { get; set; }
        [JsonProperty("giro_amount_2")]
        public decimal? GiroAmount1 { get; set; }
        [JsonProperty("giro_due_date_2")]
        public DateTime? GiroDueDate1 { get; set; }

        //GIRO 3
        [JsonProperty("giro_number_3")]
        public string GiroNumber2 { get; set; }
        [JsonProperty("giro_photo_3")]
        public string GiroPhoto2 { get; set; }
        [JsonProperty("giro_amount_3")]
        public decimal? GiroAmount2 { get; set; }
        [JsonProperty("giro_due_date_3")]
        public DateTime? GiroDueDate2 { get; set; }

        //GIRO 4
        [JsonProperty("giro_number_4")]
        public string GiroNumber3 { get; set; }
        [JsonProperty("giro_photo_4")]
        public string GiroPhoto3 { get; set; }
        [JsonProperty("giro_amount_4")]
        public decimal? GiroAmount3 { get; set; }
        [JsonProperty("giro_due_date_4")]
        public DateTime? GiroDueDate3 { get; set; }

        //GIRO 5
        [JsonProperty("giro_number_5")]
        public string GiroNumber4 { get; set; }
        [JsonProperty("giro_photo_5")]
        public string GiroPhoto4 { get; set; }
        [JsonProperty("giro_amount_5")]
        public decimal? GiroAmount4 { get; set; }
        [JsonProperty("giro_due_date_5")]
        public DateTime? GiroDueDate4 { get; set; }
        #endregion

        public IFormFile GiroPhotoAttachment { get; set; }
        public IFormFile GiroPhotoAttachment1 { get; set; }
        public IFormFile GiroPhotoAttachment2 { get; set; }
        public IFormFile GiroPhotoAttachment3 { get; set; }
        public IFormFile GiroPhotoAttachment4 { get; set; }

    }
}
