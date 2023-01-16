using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Trisatech.MWorkforce.Domain.Entities
{
    public class SalesManualReport : BaseData
    {
        public SalesManualReport()
        {
            ReportDate = DateTime.UtcNow;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime ReportDate { get; set; }
        [StringLength(50)]
        public string SalesCode { get; set; }
        [StringLength(100)]
        public string SalesName { get; set; }
        [StringLength(50)]
        public string CustomerCode { get; set; }
        [StringLength(100)]
        public string CustomerName { get; set; }
        public decimal Cash { get; set; }
        public decimal Giro { get; set; }
        public decimal Transfer { get; set; }
        public decimal Total { get; set; }
        public decimal InvoiceValue { get; set; }
        public int Jenis { get; set; }
        public decimal Nominal { get; set; }
    }
}
