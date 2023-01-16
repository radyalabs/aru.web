using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Cms.ViewModels
{
    public class DetailUserReportViewModel
    {
        public string UserId { get; set; }
        public string UserCode { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public List<VisitHistoryViewModel> VisitHistory { get; set; }
        public List<LocationHistoryViewModel> LocationHistory { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndedTime { get; set; }

        public double TotalWork { get
            {
                double result = 0;

                if (StartTime != null && EndedTime != null)
                {
                    result = Math.Round((EndedTime - StartTime).Value.TotalHours, 2);
                }
                else if (StartTime != null)
                {
                    result = Math.Round((StartTime.Date.AddHours(22) - StartTime).TotalHours, 2);
                }

                return result;
            }
        }

        public double TotalLostTime { get; set; }
        public double TotalLostTimeInMinute { get
            {
                return Math.Round(TotalLostTime / 60, 2);
            }
        }

        public double TotalKM { get; set; }
        public double StartLatitude { get; set; }
        public double EndLatitude { get; set; }
        public double StartLongitude { get; set; }
        public double EndLongitude { get; set; }
        public string LocationHistoryJson { get; set; }
        public string VisitHistoryJson { get; set; }
        public string UserWayRoutesJson { get; set; }
    }

    public partial class VisitHistoryViewModel
    {
        public string CustomerName { get; set; }
        public string TaskName { get; set; }
        public DateTime StartedTime { get; set; }
        public DateTime? EndedTime { get; set; }
        public string Status { get; set; }
        public int TotalInvoice { get; set; }
    }
}
