using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Trisatech.MWorkforce.Domain.Entities
{
    public class Account:BaseData
    {
        [Key]
        [StringLength(50, MinimumLength = 24)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string AccountId { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 64)]
        public string Password { get; set; }
        
        public string CurrentToken { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string RoleCode { get; set; }

        [ForeignKey("Role")]
        [StringLength(100)]
        public string RoleId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime LastLoginDt { get; set; }
        public bool IsPushNotifActive { get; set; }
        public string FCMToken { get; set; }
        [StringLength(255)]
        public string DeviceId { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
