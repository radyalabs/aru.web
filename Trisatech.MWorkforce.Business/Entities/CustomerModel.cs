using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using static Trisatech.AspNet.Common.Helpers.CopyProperty;

namespace Trisatech.MWorkforce.Business.Entities
{
    public class ContactModel
    {
        [JsonProperty("contact_id")]
        public string ContactId { get; set; }
        [JsonProperty("customer_code")]
        public string CustomerCode { get; set; }
        [JsonProperty("position")]
        public string Position { get; set; }
        [JsonProperty("contact_name")]
        public string ContactName { get; set; }
        [JsonProperty("contact_number")]
        public string ContactNumber { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("secondary_email")]
        public string SecondaryEmail { get; set; }
        [JsonProperty("photo")]
        public string ContactPhoto { get; set; }
        [JsonProperty("remarks")]
        public string Remarks { get; set; }
    }
    public class CustomerModel
    {
        [JsonProperty("customer_id")]
        public string CustomerId { get; set; }
        [JsonProperty("customer_email")]
        public string CustomerEmail { get; set; }
        [JsonProperty("customer_code")]
        public string CustomerCode { get; set; }
        [JsonProperty("customer_name")]
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
        [CopyPropertyIgnore]
        [JsonProperty("contact")]
        public List<ContactModel> Contacts { get; set; }
    }
}
