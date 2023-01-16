using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Cms.ViewModels
{
    public class LocationHistoryViewModel
    {
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("latitude")]
        public double? Latitude { get; set; }
        [JsonProperty("longitude")]
        public double? Longitude { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("is_active")]
        public bool IsActive {
            get
            {
                if(StartTime == null)
                {
                    return false;
                }else if(StartTime.Value.ToLocalTime().Date == DateTime.UtcNow.ToLocalTime().Date)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public DateTime? StartTime { get; set; }
        
        [JsonProperty("checkin_date")]
        public string CheckInDateTime
        {
            get
            {
                if(StartTime == null)
                {
                    return "Never Checkin";
                }
                else
                {
                    return StartTime.Value.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss");
                }
            }
        }
    }
}
