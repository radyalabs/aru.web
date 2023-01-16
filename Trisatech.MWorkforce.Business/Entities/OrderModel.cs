using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Trisatech.MWorkforce.Business.Entities
{
    public class OrderModel
    {
        [JsonProperty("order_id")]
        public string OrderId { get; set; }
        [JsonProperty("product_code")]
        public string ProductCode { get; set; }
        [JsonProperty("product_name")]
        public string ProductName { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
        [JsonProperty("product_amount")]
        public decimal ProductAmount { get; set; }
        [JsonProperty("discount")]
        public double Discount { get; set; }
        [JsonProperty("total")]
        public double Total { get; set; }

        [JsonProperty("assignment_id")]
        public string AssignmentId { get; set; }
        [JsonProperty("customer_id")]
        public string CustomerId { get; set; }
    }
}
