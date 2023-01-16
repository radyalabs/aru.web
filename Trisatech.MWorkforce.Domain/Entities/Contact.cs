using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Trisatech.MWorkforce.Domain.Entities
{
    public class Contact:BaseData
    {

        [Key]
        [StringLength(50, MinimumLength = 24)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ContactId { get; set; }
        [StringLength(40)]
        public string ContactName { get; set; }
        public string Position { get; set; }
        [StringLength(15)]
        public string ContactNumber { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(100)]
        public string SecondaryEmail { get; set; }
        [StringLength(255)]
        public string ContactPhoto { get; set; }
        [StringLength(255)]
        public string Remarks { get; set; }
        [ForeignKey("Customer")]
        [StringLength(50, MinimumLength = 24)]
        public string CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
