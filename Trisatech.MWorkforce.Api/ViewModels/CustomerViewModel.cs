using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Api.ViewModels
{
    public class CustomerViewModel
    {
        [JsonProperty("customer_id")]
        public string CustomerId { get; set; }
        [JsonProperty("customer_email")]
        [EmailAddress]
        public string CustomerEmail { get; set; }
        [JsonProperty("customer_code")]
        public string CustomerCode { get; set; }
        [JsonProperty("customer_name")]
        [Required]
        public string CustomerName { get; set; }
        [JsonProperty("customer_address")]
        public string CustomerAddress { get; set; }
        [JsonProperty("customer_phone_number")]
        public string CustomerPhoneNumber { get; set; }
        [JsonProperty("customer_city")]
        public string CustomerCity { get; set; }
        [JsonProperty("customer_photo")]
        public string CustomerPhoto { get; set; }
        [JsonProperty("customer_latitude")]
        public double? CustomerLatitude { get; set; }
        [JsonProperty("customer_longitude")]
        public double? CustomerLongitude { get; set; }
        [JsonProperty("is_contact")]
        public bool SaveAsContact { get; set; }
    }
}
