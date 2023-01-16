using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Cms.ViewModels
{
    public class CreateAssignmentViewModel
    {
        [JsonProperty("sales_id")]
        public string SalesId { get; set; }
        [JsonProperty("contact_id")]
        public string ContactId { get; set; }
        [Display(Name = "Task Name")]
        [JsonProperty("assignment_name")]
        public string AssignmentName { get; set; }
        [Display(Name = "Tanggal")]
        [JsonProperty("assignment_date")]
        public DateTime AssignmentDate { get; set; }
        [JsonProperty("address")]
        [Display(Name = "Alamat")]
        public string Address { get; set; }
        [Display(Name = "Catatan")]
        [JsonProperty("remark")]
        public string Remarks { get; set; }
        [JsonProperty("latitude")]
        public double? Latitude { get; set; }
        [JsonProperty("longitude")]
        public double? Longitude { get; set; }
    }
}
