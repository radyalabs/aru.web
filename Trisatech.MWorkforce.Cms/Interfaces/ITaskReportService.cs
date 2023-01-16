using Trisatech.MWorkforce.Cms.Models;
using Trisatech.MWorkforce.Cms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Cms.Interfaces
{
    public interface ITaskReportService
    {
        LatLng[] GetUserLocationActivity(string userid, DateTime dateTime);
        Task<bool> InsertManualReport(InsertDailyReportViewModel data, string createdBy);
        DetailUserReportViewModel GetDetailUserReport(string userid, DateTime dateTime);
        Task<int> GetManualReportCount(string keyword, DateTime startDate);
        Task<List<ViewModels.DailyReportItemViewModel>> GetManualReport(string keyword, DateTime startDate, int limit = 10, int offset = 0, string orderColumn = "UserCode", string orderType = "asc");
    }
}
