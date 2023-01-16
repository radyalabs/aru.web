using Trisatech.MWorkforce.Business.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Business.Interfaces
{
    public interface IAssignmentReportService
    {
        Task<int> GetAssingmentReportLength(string[] regions,
                    string search,
                    DateTime startDate,
                    string role = "ALL");
        Task<List<CmsReportAssignmentViewModel>> GetListAssignmentReport(string[] regions,
            string search,
            DateTime startDate,
            int length,
            int start,
            string orderBy,
            string orderType,
            string role = "ALL");
        Task<CmsDetailUserReportViewModel> GetDetail(string id, DateTime date);
    }
}
