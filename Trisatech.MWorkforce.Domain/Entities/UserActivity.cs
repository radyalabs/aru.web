using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Trisatech.MWorkforce.Domain.Entities
{
    public enum UserActivityEnum{
        START_ASSIGNMENT,
        COMPLETE_ASSIGNMENT,
        REPORT_LOCATION
    }

    public class UserActivity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserActivityId { get; set; }

        [StringLength(50, MinimumLength = 24)]
        [ForeignKey("Assignment")]
        public string AssignmentId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public UserActivityEnum ActivityTypeEnum { get; set; }
        public string AssignmentStatusCode { get; set; }
        [Required]
        public DateTime CreatedDt { get; set; }
        [Required]
        [ForeignKey("User")]
        [StringLength(50, MinimumLength = 24)]
        public string CreatedBy { get; set; }
        public virtual User User { get; set; }
        public virtual Assignment Assignment { get; set; }
    }
}
