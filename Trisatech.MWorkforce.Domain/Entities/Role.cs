using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Trisatech.MWorkforce.Domain.Entities
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [StringLength(50)]
        public string RoleId { get; set; }
        [StringLength(100)]
        public string RoleCode { get; set; }
        [StringLength(100)]
        public string RoleName { get; set; }
    }
}
