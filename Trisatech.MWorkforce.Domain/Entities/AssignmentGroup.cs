using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Trisatech.MWorkforce.Domain.Entities
{
    public class AssignmentGroup
    {
        [Key]
        [StringLength(50, MinimumLength = 24)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string AssignmentGroupId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public double? StartLongitude { get; set; }

        public double? StartLatitude { get; set; }

        public double? EndLatitude { get; set; }

        public double? EndLongitude { get; set; }

        public int TotalLostTime { get; set; }

        public double StartDistance { get; set; }

        public double EndDistance { get; set; }
        public string Reserved1 { get; set; }
        public string Reserved2 { get; set; }
        public string Reserved3 { get; set; }
        /// <summary>
        /// Attachment Id Checkin
        /// </summary>
        public string Reserved4 { get; set; }
        /// <summary>
        /// Attachment Url Checkin
        /// </summary>
        public string Reserved5 { get; set; }
        /// <summary>
        /// Attachment Id Checkout
        /// </summary>
        public string Reserved6 { get; set; }
        /// <summary>
        /// Attachment Url Checkout
        /// </summary>
        public string Reserved7 { get; set; }
        [Required]
        public DateTime CreatedDt { get; set; }

        [Required]
        [ForeignKey("User")]
        public string CreatedBy { get; set; }
        
        public virtual User User { get; set; }
    }
}
