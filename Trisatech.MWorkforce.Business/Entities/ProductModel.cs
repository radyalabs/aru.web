using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Trisatech.MWorkforce.Business.Entities
{
    public class ProductModel
    {
        [JsonProperty("product_id")]
        public string ProductId { get; set; }
        [JsonProperty("product_code")]
        public string ProductCode { get; set; }
        [JsonProperty("product_name")]
        public string ProductName { get; set; }
        [JsonProperty("product_model")]
        public string ProductType { get; set; }
        [JsonProperty("product_image")]
        public string ProductImage { get; set; }
        [JsonProperty("product_price")]
        public decimal? ProductPrice { get; set; }
    }
}
