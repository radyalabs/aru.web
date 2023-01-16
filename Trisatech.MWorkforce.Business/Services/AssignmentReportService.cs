using Trisatech.MWorkforce.Business.Entities;
using Trisatech.MWorkforce.Business.Interfaces;
using Trisatech.MWorkforce.Domain;
using Trisatech.MWorkforce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Trisatech.AspNet.Common.Interfaces;
using Trisatech.AspNet.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trisatech.AspNet.Common.Extensions;

namespace Trisatech.MWorkforce.Business.Services
{
    public class AssignmentReportService : IAssignmentReportService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly MobileForceContext _dbContext;
        public AssignmentReportService(MobileForceContext context)
        {
            unitOfWork = new UnitOfWork<MobileForceContext>(context);
            _dbContext = context;
        }

        public async Task<int> GetAssingmentReportLength(
            string[] regions,
            string search, DateTime startDate, string role = "ALL")
        {
            startDate = startDate.ToUtc();
            var endDate = startDate.AddDays(1);
            if (string.IsNullOrEmpty(search))
                search = string.Empty;

            IQueryable<string> queryCount = null;
            if(role == "DRIVER")
            {
                queryCount = (from a in _dbContext.AssignmentGroups
                              join b in _dbContext.Users
                              on a.CreatedBy equals b.UserId
                              join z in _dbContext.UserTerritories on b.UserId equals z.UserId
                              where (a.StartTime >= startDate && a.StartTime < endDate )
                              && (b.UserCode.Contains(search) || b.UserName.Contains(search))
                              && b.Account.RoleCode == role
                              && regions.Any(x=> x == z.TerritoryId)
                              //&& b.UserTerritory.Any(x => regions.Any(y => y == x.TerritoryId))
                              orderby a.StartTime ascending
                              select a.AssignmentGroupId).AsQueryable();
            }
            else
            {
                queryCount = (from a in _dbContext.AssignmentGroups
                              join b in _dbContext.Users
                              on a.CreatedBy equals b.UserId
                              join z in _dbContext.UserTerritories on b.UserId equals z.UserId
                              where (a.StartTime >= startDate && a.StartTime < endDate) &&
                              (b.UserCode.Contains(search) || b.UserName.Contains(search))
                              && regions.Any(x => x == z.TerritoryId)
                              //&& b.UserTerritory.Any(x => regions.Any(y => y == x.TerritoryId))
                              orderby a.StartTime ascending
                              select a.AssignmentGroupId).AsQueryable();
            }

            return await queryCount.CountAsync();
        }

        public async Task<CmsDetailUserReportViewModel> GetDetail(string id, DateTime date)
        {
            var repoUser = unitOfWork.GetRepository<User>();
            repoUser.Condition = PredicateBuilder.True<User>().And(x => x.UserId == id && x.IsDeleted == 0 && x.IsActive == 1);
            date = date.ToUtc();

            var user = repoUser.Find().FirstOrDefault();
            
            if (user == null)
                throw new ArgumentNullException("User ID", "Not found");

            var result = (from a in _dbContext.AssignmentGroups
                          join b in _dbContext.Users on a.CreatedBy equals b.UserId
                          join c in _dbContext.Accounts.Include(x => x.Role) on b.AccountId equals c.AccountId
                          where b.UserId == id && (a.StartTime >= date && a.StartTime < date.AddDays(1))
                          select new CmsDetailUserReportViewModel
                          {
                              UserCode = b.UserCode,
                              UserId = b.UserId,
                              Name = b.UserName,
                              UserEmail = b.UserEmail,
                              RoleName = c.Role.RoleName,
                              UserName = b.UserName,
                              UserPhone = b.UserPhone,
                              StartLatitude = a.StartLatitude ?? 0,
                              EndLatitude = a.EndLatitude ?? 0,
                              StartLongitude = a.StartLongitude ?? 0,
                              EndLongitude = a.EndLongitude ?? 0,
                              StartTime = a.StartTime,
                              EndedTime = a.EndTime,
                              TotalLossTime = a.TotalLostTime,
                              CheckInImage = a.Reserved5,
                              CheckoutImage = a.Reserved7,
                              TotalTaskCompleted = (from x in _dbContext.AssignmentDetails
                                                    where x.UserId == b.UserId
                                                    && x.Assignment.AssignmentStatusCode == ReportConstant.TASK_COMPLETED
                                                    //&& (x.EndTime != null && x.EndTime.Value.Date == a.StartTime.Date)
                                                    && (x.Assignment.AssignmentDate.Date == a.StartTime.Date)
                                                    select x.AssignmentDetailId).Count(),
                              VisitHistory = (from y in _dbContext.Assignments
                                              join assignment_detail in _dbContext.AssignmentDetails on y.AssignmentId equals assignment_detail.AssignmentId
                                              join contact in _dbContext.Contacts on assignment_detail.ContactId equals contact.ContactId
                                              join customer in _dbContext.Customers on contact.CustomerId equals customer.CustomerId  
                                              where y.AssignmentDetail.UserId == b.UserId
                                              && a.StartTime.Date == y.AssignmentDate.Date
                                              orderby y.AssignmentDetail.EndTime descending
                                              select new CmsVisitHistoryViewModel
                                              {
                                                  CustomerCode = customer.CustomerCode,
                                                  CustomerName = y.AssignmentName,
                                                  Address = y.Address,
                                                  EndedTime = y.AssignmentDetail.EndTime,
                                                  StartedTime = y.AssignmentDetail.StartTime,
                                                  VisitStatus = y.AssignmentStatus.AssignmentStatusCode,
                                                  TaskName = y.AssignmentName,
                                                  TotalInvoice = y.Invoices.Count,
                                                  InvoiceAmount = y.Invoices.Sum(x => x.Amount),
                                                  PaymentAmount = y.Payments.Sum(x => x.CashAmount + x.TransferAmount + x.GiroAmount),
                                                  IsVerified = y.AssignmentDetail.IsVerified,
                                                  GoogleTime = y.AssignmentDetail.GoogleTime,
                                                  SalesTime = y.AssignmentDetail.SalesTime,
                                                  Attachment = y.AssignmentDetail.Attachment,
                                                  Reason = y.RefAssigment != null ? $"{y.RefAssigment.Name} {y.RefAssigment.Desc}" : y.AssignmentDetail.Remarks,
                                              }
                                            ).ToList(),
                              LocationHistory = (from h in _dbContext.UserActivities
                                                 where h.Assignment.AssignmentDetail.UserId == b.UserId &&
                                                 h.AssignmentStatusCode == ReportConstant.AGENT_STARTED &&
                                                 h.Assignment.AssignmentDate.Date == a.StartTime.Date
                                                 orderby h.CreatedDt ascending
                                                 select new CmsLocationHistoryViewModel
                                                 {
                                                     Label = h.Assignment.AssignmentDetail.Contact.ContactName + ": " + h.Assignment.Address,
                                                     Latitude = h.Latitude,
                                                     Status = h.AssignmentStatusCode,
                                                     Longitude = h.Longitude,
                                                     StartTime = h.CreatedDt,
                                                     GoogleTime = h.Assignment.AssignmentDetail.GoogleTime,
                                                     SalesTime = h.Assignment.AssignmentDetail.SalesTime
                                                 }
                                              ).ToList()
                          }).AsQueryable();

            return await result.FirstOrDefaultAsync();
        }

        public async Task<List<CmsReportAssignmentViewModel>> GetListAssignmentReport(
            string[] regions,
            string search, 
            DateTime startDate,
            int length,
            int start,
            string orderBy,
            string orderType,
            string role = "ALL")
        {
            if (string.IsNullOrEmpty(search))
                search = string.Empty;

            startDate = startDate.ToUtc();
            var endDate = startDate.AddDays(1);

            IQueryable<CmsReportAssignmentViewModel> queryReport = null;
            if (role == AppConstant.Role.DRIVER){
                queryReport = (from a in _dbContext.AssignmentGroups
                               join b in _dbContext.Users on a.CreatedBy equals b.UserId
                               join z in _dbContext.UserTerritories on b.UserId equals z.UserId
                               where (a.StartTime >= startDate && a.StartTime < endDate )
                               && (b.UserName.Contains(search) || b.UserCode.Contains(search))
                               && b.Account.RoleCode == role
                               && regions.Any(x=> x == z.TerritoryId)
                               //&& b.UserTerritory.Any(x => regions.Any(y => y == x.TerritoryId))
                               orderby a.StartTime ascending
                               select new CmsReportAssignmentViewModel
                               {
                                   UserCode = b.UserCode,
                                   UserId = b.UserId,
                                   Name = b.UserName,
                                   StartTime = a.StartTime,
                                   EndedTime = a.EndTime,
                                   UserName = b.UserName,
                                   TotalLossTime = a.TotalLostTime,
                                   FirstCheckinTime = (from c in _dbContext.AssignmentDetails
                                                       where c.UserId == b.UserId
                                                       && c.Assignment.AssignmentStatusCode == ReportConstant.TASK_COMPLETED
                                                       && (c.Assignment.AssignmentDate.Date == a.StartTime.Date)
                                                       orderby c.UpdatedDt ascending
                                                       select c.StartTime).FirstOrDefault(),
                                   LostTimeAtFirstCheckin = (from c in _dbContext.AssignmentDetails
                                                             where c.UserId == b.UserId
                                                             && c.Assignment.AssignmentStatusCode == ReportConstant.TASK_COMPLETED
                                                             && (c.Assignment.AssignmentDate.Date == a.StartTime.Date)
                                                             orderby c.UpdatedDt ascending
                                                             select c.LostTime).FirstOrDefault(),
                                   TotalTaskCompleted = (from c in _dbContext.AssignmentDetails
                                                         where c.UserId == b.UserId
                                                         && c.Assignment.AssignmentStatusCode == ReportConstant.TASK_COMPLETED
                                                         //&& (c.EndTime != null && c.EndTime.Value.Date == a.StartTime.Date)
                                                         && (c.Assignment.AssignmentDate.Date == a.StartTime.Date)
                                                         select c.AssignmentDetailId).Count(),
                                   CmsTotalKunjungan = (from d in _dbContext.AssignmentDetails
                                                        where d.UserId == b.UserId
                                                        && (d.Assignment.AssignmentDate.Date == a.StartTime.Date)
                                                        select d.AssignmentId).Count(),
                                   CmsTotalPayment = (from e in _dbContext.Assignments
                                                      where e.AssignmentDetail.UserId == b.UserId
                                                      && (e.AssignmentDate.Date == a.StartTime.Date)
                                                      select e.Payments.Sum(x => x.CashAmount + x.GiroAmount + x.TransferAmount)).Sum(),
                                   ListVisit = (from f in _dbContext.Assignments
                                                where f.AssignmentDetail.UserId == b.UserId
                                                && (f.AssignmentDate.Date == a.StartTime.Date)
                                                select new CmsVisitDetail
                                                {
                                                    StartTime = f.AssignmentDetail.StartTime,
                                                    EndTime = f.AssignmentDetail.EndTime
                                                }).ToList(),
                                   TotalVerifiedTask = (from c in _dbContext.AssignmentDetails
                                                        where c.UserId == b.UserId
                                                        && c.IsVerified
                                                        && (c.Assignment.AssignmentDate.Date == a.StartTime.Date)
                                                        select c.AssignmentDetailId).Count()
                               }).Skip(start).Take(length).AsQueryable();
            }
            else {
                queryReport = (from a in _dbContext.AssignmentGroups
                               join b in _dbContext.Users
                               on a.CreatedBy equals b.UserId
                               join z in _dbContext.UserTerritories on b.UserId equals z.UserId
                               where (a.StartTime >= startDate && a.StartTime < endDate )
                               && (b.UserName.Contains(search) || b.UserCode.Contains(search))
                               && regions.Any(x=> x == z.TerritoryId)
                               orderby a.StartTime ascending
                               select new CmsReportAssignmentViewModel
                               {
                                   UserCode = b.UserCode,
                                   UserId = b.UserId,
                                   Name = b.UserName,
                                   StartTime = a.StartTime,
                                   EndedTime = a.EndTime,
                                   UserName = b.UserName,
                                   TotalLossTime = a.TotalLostTime,
                                   FirstCheckinTime = (from c in _dbContext.AssignmentDetails
                                                       where c.UserId == b.UserId
                                                       && c.Assignment.AssignmentStatusCode == ReportConstant.TASK_COMPLETED
                                                       && (c.Assignment.AssignmentDate.Date == a.StartTime.Date)
                                                       orderby c.UpdatedDt ascending
                                                       select c.StartTime).FirstOrDefault(),
                                   LostTimeAtFirstCheckin = (from c in _dbContext.AssignmentDetails
                                                             where c.UserId == b.UserId
                                                             && c.Assignment.AssignmentStatusCode == ReportConstant.TASK_COMPLETED
                                                             && (c.Assignment.AssignmentDate.Date == a.StartTime.Date)
                                                             orderby c.UpdatedDt ascending
                                                             select c.LostTime).FirstOrDefault(),
                                   TotalTaskCompleted = (from c in _dbContext.AssignmentDetails
                                                         where c.UserId == b.UserId
                                                         && c.Assignment.AssignmentStatusCode == ReportConstant.TASK_COMPLETED
                                                         //&& (c.EndTime != null && c.EndTime.Value.Date == a.StartTime.Date)
                                                         && (c.Assignment.AssignmentDate.Date == a.StartTime.Date)
                                                         select c.AssignmentDetailId).Count(),
                                   CmsTotalKunjungan = (from d in _dbContext.AssignmentDetails
                                                        where d.UserId == b.UserId
                                                        && (d.Assignment.AssignmentDate.Date == a.StartTime.Date)
                                                        select d.AssignmentId).Count(),
                                   CmsTotalPayment = (from e in _dbContext.Assignments
                                                      where e.AssignmentDetail.UserId == b.UserId
                                                      && (e.AssignmentDate.Date == a.StartTime.Date)
                                                      select e.Payments.Sum(x => x.CashAmount + x.GiroAmount + x.TransferAmount)).Sum(),
                                   ListVisit = (from f in _dbContext.Assignments
                                                where f.AssignmentDetail.UserId == b.UserId
                                                && (f.AssignmentDate.Date == a.StartTime.Date)
                                                select new CmsVisitDetail
                                                {
                                                    StartTime = f.AssignmentDetail.StartTime,
                                                    EndTime = f.AssignmentDetail.EndTime
                                                }).ToList(),
                                   TotalVerifiedTask = (from c in _dbContext.AssignmentDetails
                                                        where c.UserId == b.UserId
                                                        && c.IsVerified
                                                        && (c.Assignment.AssignmentDate.Date == a.StartTime.Date)
                                                        select c.AssignmentDetailId).Count()
                               }).Skip(start).Take(length).AsQueryable();
            }
            
            return await queryReport.ToListAsync();
        }
    }
}
