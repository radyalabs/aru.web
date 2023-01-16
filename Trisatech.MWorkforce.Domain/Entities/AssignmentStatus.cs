using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Trisatech.MWorkforce.Domain.Entities
{
    public class AssignmentStatus
    {
        [Key]
        [StringLength(20, MinimumLength = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string AssignmentStatusCode { get; set; }

        [Required]
        [StringLength(50)]
        public string AssignmentStatusName { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
    }
}
