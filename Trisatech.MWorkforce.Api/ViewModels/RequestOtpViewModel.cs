using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Api.ViewModels
{
    public class RequestOtpViewModel
    {
        [JsonProperty("payment_method")]
        public string PaymentMethod { get; set; }
        [JsonProperty("total_payment")]
        public decimal TotalPembayaran { get; set; }
    }
}
