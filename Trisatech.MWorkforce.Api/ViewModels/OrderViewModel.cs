using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Api.ViewModels
{
    public class OrderViewModel
    {
        [JsonRequired]
        [JsonProperty("assignment_id")]
        public string AssignmentId { get; set; }

        [JsonProperty("customer_id")]
        public string CustomerId { get; set; }

        [JsonProperty("products")]
        public List<OrderItemViewModel> Products { get; set; }
    }

    public partial class OrderItemViewModel
    {
        [JsonRequired]
        [JsonProperty("product_code")]
        public string ProductCode { get; set; }
        [JsonProperty("product_name")]
        public string ProductName { get; set; }
        [JsonRequired]
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
        [JsonRequired]
        [JsonProperty("product_amount")]
        public decimal ProductAmount { get; set; }
        [JsonProperty("discount")]
        public double Discount { get; set; }

    }
}
