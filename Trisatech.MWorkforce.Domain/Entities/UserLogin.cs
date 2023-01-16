using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Trisatech.MWorkforce.Domain.Entities
{
    public class UserLogin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserLoginId { get; set; }

        [ForeignKey("Account")]
        [StringLength(50, MinimumLength = 24)]
        public string AccountId { get; set; }

        public string GeneratedToken { get; set; }

        [Required]
        public DateTime CreatedDt { get; set; }

        public DateTime? UpdatedDt { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 24)]
        public string CreatedBy { get; set; }

        [StringLength(50, MinimumLength = 24)]
        public string UpdatedBy { get; set; }
        
        public virtual Account Account { get; set; }

    }
}
