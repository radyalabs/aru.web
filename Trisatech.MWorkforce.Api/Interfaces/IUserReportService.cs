using Trisatech.MWorkforce.Api.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Api.Interfaces
{
    public interface IUserReportService
    {
        DashboardViewModel GetDashbord(string userId, DateTime dateTime);
        List<ReportAssignmentViewModel> GetListAssignmentReport(List<string> users, DateTime startDate, int limit = 10, int offset = 0, string orderColumn = "UserCode", string orderType = "desc");
        DetailUserReportViewModel GetDetailAssignmentReport(string userid, DateTime dateTime);
    }
}
