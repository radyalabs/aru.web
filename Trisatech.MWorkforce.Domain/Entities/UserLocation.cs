using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Trisatech.MWorkforce.Domain.Entities
{
    public class UserLocation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserLocationId { get; set; }

        [Required]
        public string UserCode { get; set; }
        
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Reserved1 { get; set; }
        public string Reserved2 { get; set; }

        [Required]
        public DateTime CreatedDt { get; set; }
        
        [Required]
        [StringLength(50, MinimumLength = 24)]
        public string CreatedBy { get; set; }
    }
}
