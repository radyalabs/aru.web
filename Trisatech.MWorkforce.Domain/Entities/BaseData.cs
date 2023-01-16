using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Trisatech.MWorkforce.Domain.Entities
{
    public class BaseData
    {
        [Required]
        public DateTime CreatedDt { get; set; }

        public DateTime? UpdatedDt { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 24)]
        public string CreatedBy { get; set; }

        [StringLength(50, MinimumLength = 24)]
        public string UpdatedBy { get; set; }

        public byte IsActive { get; set; }

        public byte IsDeleted { get; set; }
    }
}
