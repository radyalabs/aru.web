using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Cms.ViewModels
{
    public class ConvertTaskViewModel
    {
        public List<TaskUploadViewModel> TaskUploadViewModel { get; private set; }
        public ConvertTaskViewModel(IEnumerable<Dictionary<string, object>> data)
        {
            TaskUploadViewModel = new List<TaskUploadViewModel>();
            foreach(var item in data)
            {
                ViewModels.TaskUploadViewModel newTaskItem = new TaskUploadViewModel
                {
                    Sales = Convert.ToString(item["sales"]),
                    AssignmentDate = Convert.ToString(item["tanggal"]),
                    AssignmentDueDate = Convert.ToString(item["jatuh tempo"]),
                    CustomerCode = Convert.ToString(item["customer code"]),
                    CustomerName = Convert.ToString(item["customer name"]),
                    NoFaktur = Convert.ToString(item["no faktur"]),
                    Amount = Convert.ToString(item["jumlah"]),
                    Address = Convert.ToString(item["alamat"]),
                    Phone = Convert.ToString(item["telepon"]),
                    Latitude = (string.IsNullOrEmpty(Convert.ToString(item["latitude"])) ? 0 : Convert.ToDouble(item["latitude"], new CultureInfo("en-US"))),
                    Longitude = (string.IsNullOrEmpty(Convert.ToString(item["longitude"])) ? 0 : Convert.ToDouble(item["longitude"], new CultureInfo("en-US")))
                };

                if (TaskUploadViewModel.Where(x => x.CustomerCode == newTaskItem.CustomerCode && x.NoFaktur == newTaskItem.NoFaktur && x.AssignmentDate == newTaskItem.AssignmentDate).FirstOrDefault() == null)
                {
                    TaskUploadViewModel.Add(newTaskItem);
                }
            }
        }
    }

    public class TaskDataUploadViewModel
    {
        public List<TaskAttachDetailViewModel> data { get; set; }
    }

    public class TaskAttachDetailViewModel
    {
        [JsonProperty("sales")]
        public string Sales { get; set; }
        [JsonProperty("assignment_date")]
        public DateTime AssignmentDate { get; set; }
        [JsonProperty("assignment_due_date")]
        public DateTime? AssignmentDueDate { get; set; }
        [JsonProperty("customer_code")]
        public string CustomerCode { get; set; }
        [JsonProperty("customer_name")]
        public string CustomerName { get; set; }
        [JsonProperty("no_faktur")]
        public string NoFaktur { get; set; }
        [JsonProperty("amount")]
        public int Amount { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }

    public class TaskUploadViewModel
    {
        [JsonProperty("sales")]
        public string Sales { get; set; }
        [JsonProperty("assignment_date")]
        public string AssignmentDate { get; set; }
        [JsonProperty("assignment_due_date")]
        public string AssignmentDueDate { get; set; }
        [JsonProperty("customer_code")]
        public string CustomerCode { get; set; }
        [JsonProperty("customer_name")]
        public string CustomerName { get; set; }
        [JsonProperty("no_faktur")]
        public string NoFaktur { get; set; }
        [JsonProperty("amount")]
        public string Amount { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }
}
