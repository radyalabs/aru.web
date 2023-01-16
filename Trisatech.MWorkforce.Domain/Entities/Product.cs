using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Trisatech.MWorkforce.Domain.Entities
{
    public class Product:BaseData
    {

        [Key]
        [StringLength(50, MinimumLength = 24)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ProductId { get; set; }

        [StringLength(30, MinimumLength = 4)]
        public string ProductCode { get; set; }

        [StringLength(255, MinimumLength = 3)]
        public string ProductName { get; set; }

        [StringLength(255)]
        public string ProductModel { get; set; }

        public decimal? ProductPrice { get; set; }

        [StringLength(255, MinimumLength = 12)]
        public string ProductImage { get; set; }
    }
}
