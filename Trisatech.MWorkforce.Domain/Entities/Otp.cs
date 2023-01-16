using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Trisatech.MWorkforce.Domain.Entities
{
    public class Otp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OtpId { get; set; }
        public string Type { get; set; }
        [StringLength(50, MinimumLength = 24)]
        public string ItemId { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 6)]
        public string Value { get; set; }
        public bool IsValid { get; set; }
        public DateTime ValidTime { get; set; }
        [Required]
        public DateTime CreatedDt { get; set; }
        [Required]
        public string CreatedBy { get; set; }
    }
}
