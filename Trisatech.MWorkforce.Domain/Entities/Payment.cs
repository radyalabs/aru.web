using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Trisatech.MWorkforce.Domain.Entities
{
    public enum PaymentChannel
    {
        Cash, Transfer, Giro, CashAndTransfer, CashAndGiro, TransferAndGiro, CashAndTransferAndGiro
    }

    public class Payment : BaseData
    {
        [Key]
        [StringLength(50, MinimumLength = 24)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string PaymentId { get; set; }
        [StringLength(100)]
        public string InvoiceCode { get; set; }
        public string AssignmentCode { get; set; }
        [ForeignKey("Assignment")]
        [StringLength(50, MinimumLength = 24)]
        public string AssignmentId { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal PaymentDebt { get; set; }
        public PaymentChannel PaymentChannel { get; set; }
        
        [StringLength(255)]
        public string TransferBank { get; set; }
        public decimal TransferAmount { get; set; }
        public DateTime? TransferDate { get; set; }
        [StringLength(100)]
        public string GiroNumber { get; set; }
        [StringLength(255)]
        public string GiroPhoto { get; set; }
        public decimal GiroAmount { get; set; }
        public DateTime? GiroDueDate { get; set; }
        public decimal CashAmount { get; set; }
        public string BlobName { get; set; }
        public virtual Assignment Assignment { get; set; }
    }
}
