using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Trisatech.MWorkforce.Domain.Entities
{
    public class RefAssigment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(6)]
        public string Code { get; set; }
        [StringLength(3)]
        public string GroupId { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Status { get; set; }
        [StringLength(255)]
        public string Desc { get; set; }
        public byte IsDeleted { get; set; }
    }
}
