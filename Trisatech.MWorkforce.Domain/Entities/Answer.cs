using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Trisatech.MWorkforce.Domain.Entities
{
    public class Answer:BaseData
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnswerId { get; set; }

        [Required]
        [StringLength(100)]
        public string AssignmentCode { get; set; }

        [ForeignKey("Assignment")]
        [StringLength(50, MinimumLength = 24)]
        public string AssignmentId { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        [Required]
        [ForeignKey("Survey")]
        public string SurveyId { get; set; }
        
        public virtual Survey Survey { get; set; }
        public virtual User User { get; set; }
        public virtual Assignment Assignment { get; set; }
    }
}
