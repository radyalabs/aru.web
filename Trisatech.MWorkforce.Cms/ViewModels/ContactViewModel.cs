using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Cms.ViewModels
{
    public class ContactViewModel
    {
        [JsonProperty("contact_id")]
        public string ContactId { get; set; }
        [JsonProperty("customer_code")]
        [Display(Name = "Customer Code")]
        public string CustomerCode { get; set; }
        [JsonProperty("position")]
        [Display(Name = "Position")]
        public string Position { get; set; }
        [Required]
        [JsonProperty("contact_name")]
        [Display(Name = "Name")]
        public string ContactName { get; set; }
        [Required]
        [JsonProperty("contact_number")]
        [Display(Name = "Phone/Mobile Phone")]
        public string ContactNumber { get; set; }
        [JsonProperty("email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [JsonProperty("secondary_email")]
        [Display(Name = "Secondary Email")]
        public string SecondaryEmail { get; set; }
        [JsonProperty("photo")]
        public string ContactPhoto { get; set; }
        [JsonProperty("remarks")]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }
    }
}
