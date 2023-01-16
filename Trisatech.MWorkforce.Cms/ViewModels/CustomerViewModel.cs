using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Cms.ViewModels
{
    public class CustomerViewModel
    {
        [JsonProperty("customer_id")]
        public string CustomerId { get; set; }
        [JsonProperty("customer_email")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string CustomerEmail { get; set; }
        [JsonProperty("customer_code")]
        public string CustomerCode { get; set; }
        [JsonProperty("customer_name")]
        [Required]
        [Display(Name = "Name")]
        public string CustomerName { get; set; }
        [JsonProperty("customer_address")]
        [Display(Name = "Address")]
        public string CustomerAddress { get; set; }
        [JsonProperty("customer_phone_number")]
        [Display(Name = "Phone")]
        public string CustomerPhoneNumber { get; set; }
        [JsonProperty("customer_city")]
        [Display(Name = "City")]
        public string CustomerCity { get; set; }
        [JsonProperty("customer_photo")]
        public string CustomerPhoto { get; set; }
        [JsonProperty("customer_latitude")]
        public double? CustomerLatitude { get; set; }
        [JsonProperty("customer_longitude")]
        public double? CustomerLongitude { get; set; }
        [Display(Name = "Add as contact")]
        public bool SaveAsContact { get; set; }
        [JsonProperty("customer_district")]
        public string CustomerDistrict;
        [JsonProperty("customer_photo_npwp")]
        public string CustomerPhotoNPWP;
        [JsonProperty("customer_village")]
        public string CustomerVillage;
        [JsonProperty("desc")]
        [Display(Name = "Notes")]
        public string desc;
    }
}
