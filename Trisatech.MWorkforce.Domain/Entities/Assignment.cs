using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Trisatech.MWorkforce.Domain.Entities
{
    public enum AssignmentType { VISIT, GASOLINE, CICO }

    public class Assignment:BaseData
    {
        [Key]
        [StringLength(50, MinimumLength = 24)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string AssignmentId { get; set; }
        [StringLength(100)]
        public string AssignmentCode { get; set; }
        [StringLength(255)]
        public string AssignmentName { get; set; }
        [Required]
        [ForeignKey("AssignmentStatus")]
        public string AssignmentStatusCode { get; set; }
        [ForeignKey("RefAssignment")]
        public string RefAssignmentCode { get; set; }
        public DateTime AssignmentDate { get; set; }
        public DateTime? AssignmentDueDate { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        [StringLength(200)]
        public string Address { get; set; }
        [StringLength(255)]
        public string Remarks { get; set; }
        public AssignmentType AssignmentType { get; set; }

        public virtual AssignmentDetail AssignmentDetail { get; set; }
        public virtual AssignmentStatus AssignmentStatus { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual RefAssigment RefAssigment { get; set; }
    }
}
