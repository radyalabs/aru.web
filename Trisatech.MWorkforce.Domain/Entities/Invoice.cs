using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trisatech.MWorkforce.Domain.Entities
{
    public class Invoice:BaseData
    {
        [Key]
        [StringLength(50, MinimumLength = 24)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string InvoiceId { get; set; }

        public string InvoiceCode { get; set; }

        public string AssignmentCode { get; set; }
        public DateTime? DueDate { get; set; }
        [ForeignKey("Assignment")]
        [StringLength(50, MinimumLength = 24)]
        public string AssignmentId { get; set; }
        public decimal Amount { get; set; }
        
        [StringLength(50)]
        public string Status { get; set; }

        public virtual Assignment Assignment { get; set; }
    }
}
