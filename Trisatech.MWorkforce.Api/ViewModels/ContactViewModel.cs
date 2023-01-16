using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Api.ViewModels
{
    public class ContactViewModel
    {
        [JsonProperty("contact_id")]
        public string ContactId { get; set; }
        [JsonProperty("position")]
        public string Position { get; set; }
        [Required]
        [JsonProperty("contact_name")]
        public string ContactName { get; set; }
        [Required]
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
}
