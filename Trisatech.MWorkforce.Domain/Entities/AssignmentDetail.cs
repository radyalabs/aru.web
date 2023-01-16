using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Trisatech.MWorkforce.Domain.Entities
{
    public class AssignmentDetail:BaseData
    {
        [Key]
        [StringLength(50, MinimumLength = 24)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string AssignmentDetailId { get; set; }
        
        [ForeignKey("Assignment")]
        [StringLength(50, MinimumLength = 24)]
        public string AssignmentId { get; set; }

        [ForeignKey("User")]
        [StringLength(50, MinimumLength = 24)]
        public string UserId { get; set; }
        [StringLength(50)]

        public string UserCode { get; set; }

        [ForeignKey("Contact")]
        public string ContactId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int LostTime { get; set; }

        public int SalesTime { get; set; }
        public int GoogleTime { get; set; }
        public bool IsVerified { get; set; }
        [StringLength(255)]
        public string Remarks { get; set; }
        [StringLength(500)]
        public string Attachment { get; set; }
        [StringLength(255)]
        public string AttachmentBlobId { get; set; }
        [StringLength(500)]
        public string Attachment1 { get; set; }
        [StringLength(255)]
        public string AttachmentBlobId1 { get; set; }
        [StringLength(500)]
        public string Attachment2 { get; set; }
        [StringLength(255)]
        public string AttachmentBlobId2 { get; set; }
        public virtual User User { get; set; }
        public virtual Assignment Assignment { get; set; }
        public virtual Contact Contact { get; set; }

        public static implicit operator AssignmentDetail(HashSet<AssignmentDetail> v)
        {
            throw new NotImplementedException();
        }
    }
}
