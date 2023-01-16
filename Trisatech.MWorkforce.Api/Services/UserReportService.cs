using Trisatech.MWorkforce.Api.Interfaces;
using Trisatech.MWorkforce.Api.ViewModels;
using Trisatech.MWorkforce.Domain.Entities;
using Trisatech.MWorkforce.Domain;
using Microsoft.EntityFrameworkCore;
using Trisatech.AspNet.Common.Interfaces;
using Trisatech.AspNet.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Api.Services
{
    public class UserReportService : IUserReportService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepository<Assignment> repoAssignment;
        private readonly MobileForceContext _dbContext;
        public UserReportService(MobileForceContext context)
        {
            unitOfWork = new UnitOfWork<MobileForceContext>(context);
            _dbContext = context;
            repoAssignment = this.unitOfWork.GetRepository<Assignment>();
        }
        
        public DashboardViewModel GetDashbord(string userId, DateTime dateTime)
        {
            DashboardViewModel result = new DashboardViewModel();

            try
            {
                repoAssignment.Includes = new string[] { "AssignmentDetail" };
                repoAssignment.Condition = PredicateBuilder.True<Assignment>()
                    .And(x => x.IsDeleted == 0
                    && x.AssignmentDetail.UserId == userId 
                    && (x.AssignmentDate >= dateTime && x.AssignmentDate < dateTime.AddDays(1)));

                var allAssignment = repoAssignment.Find().ToList();

                if(allAssignment!=null && allAssignment.Count > 0)
                {
                    result.TotalTask = allAssignment.Count;
                    result.Failed = allAssignment.Count(x => x.AssignmentStatusCode == "TASK_FAILED");
                    result.Success = allAssignment.Count(x => x.AssignmentStatusCode == "TASK_COMPLETED");
                    result.OnGoing = allAssignment.Count(x => x.AssignmentStatusCode != "TASK_FAILED" && x.AssignmentStatusCode != "TASK_COMPLETED");
                }

            }catch(ApplicationException appEx)
            {
                throw appEx;
            }catch(Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public DetailUserReportViewModel GetDetailAssignmentReport(string userid, DateTime dateTime)
        {
            try
            {
                var repoUser = unitOfWork.GetRepository<User>();
                repoUser.Condition = PredicateBuilder.True<User>().And(x => x.UserId == userid && x.IsDeleted == 0 && x.IsActive == 1);

                var user = repoUser.Find().FirstOrDefault();
                if (user == null)
                    throw new ApplicationException("not found");

                var result = (from a in _dbContext.AssignmentGroups
                              join b in _dbContext.Users on a.CreatedBy equals b.UserId
                              join c in _dbContext.Accounts.Include(x => x.Role) on b.AccountId equals c.AccountId
                              where b.UserId == userid 
                              && (a.StartTime >= dateTime && a.StartTime < dateTime.AddDays(1))
                              select new DetailUserReportViewModel
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
                                  TotalTaskCompleted = (from x in _dbContext.AssignmentDetails
                                                        where x.UserId == b.UserId
                                                        && (x.Assignment.AssignmentStatusCode == AppConstant.AssignmentStatus.TASK_COMPLETED)
                                                        && (x.EndTime != null && x.EndTime.Value.Date == a.StartTime.Date)
                                                        select x).Count(),
                                  VisitHistory = (from y in _dbContext.Assignments
                                                  where y.AssignmentDetail.UserId == b.UserId 
                                                  && a.StartTime.Date == y.AssignmentDate.Date
                                                  orderby y.AssignmentDetail.EndTime descending
                                                  select new VisitHistoryViewModel
                                                  {
                                                      CustomerName = y.AssignmentName,
                                                      EndedTime = y.AssignmentDetail.EndTime,
                                                      StartedTime = y.AssignmentDetail.StartTime,
                                                      VisitStatus = y.AssignmentStatus.AssignmentStatusCode,
                                                      TaskName = y.AssignmentName,
                                                      TotalInvoice = y.Invoices.Count,
                                                      InvoiceAmount = y.Invoices.Sum(x=> x.Amount),
                                                      PaymentAmount = y.Payments.Sum(x=>x.CashAmount + x.TransferAmount + x.GiroAmount),
                                                      IsVerified = y.AssignmentDetail.IsVerified,
                                                      GoogleTime = y.AssignmentDetail.GoogleTime,
                                                      SalesTime = y.AssignmentDetail.SalesTime
                                                  }
                                                  ).ToList(),
                                  LocationHistory = (from h in _dbContext.AssignmentDetails
                                                     join i in _dbContext.UserActivities on h.AssignmentId equals i.AssignmentId into userActivity
                                                     from i in userActivity.DefaultIfEmpty()
                                                     where h.UserId == b.UserId && 
                                                     h.Assignment.AssignmentStatusCode == AppConstant.AssignmentStatus.TASK_COMPLETED &&
                                                     i.AssignmentStatusCode == AppConstant.AssignmentStatus.TASK_COMPLETED &&
                                                     h.Assignment.AssignmentDate.Date == a.StartTime.Date
                                                     orderby i.CreatedDt descending
                                                     select new LocationHistoryViewModel
                                                     {
                                                         Label = h.Contact.ContactName + ": " + h.Assignment.Address,
                                                         Latitude = i.Latitude,
                                                         Status = i.AssignmentStatusCode,
                                                         Longitude = i.Longitude,
                                                         StartTime = i.CreatedDt,
                                                         GoogleTime = h.GoogleTime,
                                                         SalesTime = h.SalesTime
                                                     }
                                                  ).ToList()
                              }).FirstOrDefault();

                return result;

            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ReportAssignmentViewModel> GetListAssignmentReport(List<string> users, 
            DateTime date, 
            int limit = 10, 
            int offset = 0, 
            string orderColumn = "UserCode", 
            string orderType = "desc")
        {
            try
            {
                var resultDb = (from a in _dbContext.AssignmentGroups
                                join b in _dbContext.Users on a.CreatedBy equals b.UserId
                                where users.Contains(b.UserId) 
                                && (a.StartTime >= date && a.StartTime < date.AddDays(1))
                                orderby a.StartTime ascending
                                select new ReportAssignmentViewModel
                                {
                                    UserCode = b.UserCode,
                                    UserId = b.UserId,
                                    Name = b.UserName,
                                    StartTime = a.StartTime,
                                    EndedTime = a.EndTime,
                                    UserName = b.UserName,
                                    TotalLossTime = a.TotalLostTime,
                                    TotalTaskCompleted = (from c in _dbContext.AssignmentDetails
                                                 where c.UserId == b.UserId 
                                                 && (/*c.Assignment.AssignmentStatusCode == "AGENT_STARTED" ||*/ 
                                                 c.Assignment.AssignmentStatusCode == "TASK_COMPLETED") 
                                                 && (c.EndTime != null && c.EndTime.Value.Date == a.StartTime.Date)
                                                 select c).Count(),
                                    ListVisit = (from e in _dbContext.Assignments
                                                where e.AssignmentDetail.UserId == b.UserId 
                                                && a.StartTime.Date == e.AssignmentDate.Date
                                                select new VisitDetail
                                                {
                                                    AssignmentId = e.AssignmentId,
                                                    StartTime = e.AssignmentDetail.StartTime,
                                                    AssignmentDate = e.AssignmentDate,
                                                    EndTime = e.AssignmentDetail.EndTime,
                                                    InvoiceAmount = e.Invoices.Sum(x=> x.Amount),
                                                    PaymentAmount = e.Payments.Sum(x=>x.CashAmount + x.GiroAmount + x.TransferAmount),
                                                }).ToList(),
                                    
                                }).Skip(offset).Take(limit).ToList();

                return resultDb;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
