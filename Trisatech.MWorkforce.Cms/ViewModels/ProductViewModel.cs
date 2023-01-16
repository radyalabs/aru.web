using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Cms.ViewModels
{
    public class ProductViewModel
    {
        public string ProductId { get; set; }
        
        [Display(Name = "Kode")]
        public string ProductCode { get; set; }
        [Required]
        [Display(Name = "Nama")]
        public string ProductName { get; set; }
        [Display(Name = "Model")]
        public string ProductModel { get; set; }

        [Display(Name = "Harga")]
        public decimal? ProductPrice { get; set; }
    }
}
