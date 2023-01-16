using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Trisatech.MWorkforce.Domain.Entities
{
    public class Order:BaseData
    {
        [Key]
        [StringLength(50, MinimumLength = 24)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string OrderId { get; set; }
        public string AssignmentCode { get; set; }
        [ForeignKey("Assignment")]
        [StringLength(50, MinimumLength = 24)]
        public string AssignmentId { get; set; }
        
        [StringLength(100)]
        public string ProductCode { get; set; }
        [StringLength(255)]
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal ProductAmount { get; set; }
        public double Discount { get; set; }
        [StringLength(100)]
        public string CustomerCode { get; set; }
        [ForeignKey("Customer")]
        [StringLength(50, MinimumLength = 24)]
        public string CustomerId { get; set; }
    }
}
