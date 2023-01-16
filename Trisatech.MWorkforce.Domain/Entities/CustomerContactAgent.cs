using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Trisatech.MWorkforce.Domain.Entities
{
    public class CustomerContactAgent:BaseData
    {
        [Key]
        [StringLength(50, MinimumLength = 24)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string CustomerContactAgentId { get; set; }

        [StringLength(50, MinimumLength = 24)]
        public string ContactId { get; set; }

        [StringLength(50, MinimumLength = 24)]
        public string SalesId { get; set; }
        
    }
}
