using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Trisatech.MWorkforce.Cms.ViewModels
{
    public class InsertDailyReportViewModel
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("data")]
        public List<DailyReportItemViewModel> Data { get; set; }
    }

    public class DailyReportItemViewModel {
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("userCode")]
        public string UserCode { get; set; }
        [JsonProperty("userName")]
        public string UserName { get; set; }
        [JsonProperty("outletCode")]
        public string CustomerCode { get; set; }
        [JsonProperty("outletName")]
        public string CustomerName { get; set; }
        [JsonProperty("paymentCash")]
        public decimal PaymentCash { get; set; }
        [JsonProperty("paymentTransfer")]
        public decimal PaymentTransfer { get; set; }
        [JsonProperty("paymentGiro")]
        public decimal PaymentGiro { get; set; }
        [JsonProperty("orderType")]
        public decimal OrderType { get; set; }
        [JsonProperty("orderNominal")]
        public decimal OrderNominal { get; set; }
        [JsonProperty("invoiceValue")]
        public decimal InvoiceValue { get; set; }
        [JsonProperty("paymentTotal")]
        public string PaymentTotal {
            get {
                return (PaymentCash + PaymentTransfer + PaymentGiro).ToString("C", new CultureInfo("id-ID"));
            }
        }
        [JsonProperty("salesTotal")]
        public string SalesTotal{
            get {
                return (OrderType + OrderNominal).ToString("C", new CultureInfo("id-ID"));
            }
        }
    }
}
