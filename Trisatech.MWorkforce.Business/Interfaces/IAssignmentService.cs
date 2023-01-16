using Trisatech.MWorkforce.Business.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Business.Interfaces
{
    public interface IAssignmentService
    {
        void Add(AssignmentModel assignment, string createdBy, bool withInvoice = true);
        void Add(List<AssignmentModel> assignments, string createdBy, bool invoice = true);
        void Delete(string id, string deletedBy);
        void Edit(AssignmentModel assignment);
		List<AssignmentModel> GetListAssignment(string keyword, int limit = 10, int offset = 0, string orderColumn = "AssignmentCode", string orderType = "desc", DateTime? startDate = null, DateTime? endDate = null, string status = null, UserAuthenticated user = null, string role = "");
		long GetListAssignmentCount(string keyword, string orderColumn = "AssignmentCode", string orderType = "desc", string status = null, DateTime? startDate = null, DateTime? endDate = null, string role = "");
        Task<List<ReportAssignmentModel>> GetListAssignmentReport(string keyword, DateTime startDate, int limit = 10, int offset = 0, string orderColumn = "UserCode", string orderType = "asc");
        Task<long> GetListAssignmentReportCount(string keyword, DateTime date, string orderColumn = "UserCode", string orderType = "asc");
        List<AssignmentModel> GetAssignments(string user, int status = 0, int limit = 10, int offset = 0, string orderColumn = "AssignmentDate", string orderType = "desc", DateTime? from = null, DateTime? to = null);
        long? GetAssignmentsCount(string userid, int status = 0, int limit = 10, int offset = 0, string orderColumn = "AssignmentDate", string orderType = "desc", DateTime? from = null, DateTime? to = null);

        List<AssignmentModel> GetAssignmentDetail(string user, int status = 0, int limit = 10, int offset = 0, string orderColumn = "AssignmentDate", string orderType = "desc", DateTime? from = null, DateTime? to = null);
        List<AssignmentModel> GetAssignmentsByDate(string user, DateTime date, DateTime? to, short status = 0, int limit = 10, int offset = 0);
        List<AssignmentModel> Search(string user, string keywords, short status = 0, DateTime? date = null, int limit = 10, int offset = 0);

        AssignmentModel Detail(string userId = "", string id = "");
        List<AssignmentStatusModel> GetAssignmentStatus();
        void Start(string userId, 
            string assignmentId, 
            double latitude, 
            double longitude, 
            DateTime startTime, 
            string assignmentStatus, 
            int lostTime = 0, 
            int salesTime = 0, 
            int googleTime = 0);
        void Complete(string userId, string assignment_id, double latitude, double longitude, string remarks, DateTime now, string assignmentStatus, string reasonCode = "", string attachmentUrl = "", string attachmentBlobId = "");
        void CreateAssignment(CreateGasolineModel createGasolineModel, string createdBy);
        bool AllowToStartAssignment(string userId, DateTime utcNow);
        Task CopyTask(string userId, string targetUserId, DateTime utcNow);
    }
}
