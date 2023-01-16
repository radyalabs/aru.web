using Trisatech.MWorkforce.Domain;
using Trisatech.MWorkforce.Domain.Entities;
using Trisatech.MWorkforce.Cms.Interfaces;
using Trisatech.MWorkforce.Cms.Models;
using Trisatech.MWorkforce.Cms.ViewModels;
using Microsoft.EntityFrameworkCore;
using Trisatech.AspNet.Common.Interfaces;
using Trisatech.AspNet.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Cms.Services
{
    public class TaskReportService : ITaskReportService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly MobileForceContext dbContext;
        public TaskReportService(MobileForceContext context)
        {
            dbContext = context;
            unitOfWork = new UnitOfWork<MobileForceContext>(context);
        }

        public LatLng[] GetUserLocationActivity(string userid, DateTime dateTime)
        {
            try
            {
                dateTime = dateTime.ToUtc();

                var repoUserAct = unitOfWork.GetRepository<UserActivity>();
                repoUserAct.Condition = PredicateBuilder.True<UserActivity>()
                .And(x => x.CreatedBy == userid
                && (x.CreatedDt >= dateTime && x.CreatedDt < dateTime.AddDays(1)) 
                && ( x.Latitude != 0 && x.Longitude != 0));
                repoUserAct.OrderBy = new Trisatech.AspNet.Common.Models.SqlOrderBy { Column = "CreatedDt", Type = "asc" };

                var result = repoUserAct.Find<LatLng>(x => new LatLng
                {
                    Latitude = x.Latitude,
                    Longitude = x.Longitude
                }).ToArray();

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

        public async Task<bool> InsertManualReport(InsertDailyReportViewModel data, string createdBy){
            var userData = await dbContext.Users.FirstOrDefaultAsync(x=>x.UserId == data.UserId);

            if(userData == null){
                throw new InvalidOperationException("User yang akan anda report tidak ditemukan");
            }

            DateTime utcNow = DateTime.UtcNow;

            List<SalesManualReport> newSalesManualReports = new List<SalesManualReport>();
            newSalesManualReports = data.Data.Select(x=> new SalesManualReport()
            {
                SalesCode = userData.UserCode,
                SalesName = userData.FullName,
                CustomerCode = x.CustomerCode,
                CustomerName = x.CustomerName.Replace("+", " "),
                InvoiceValue = x.InvoiceValue,
                Cash = x.PaymentCash,
                Giro = x.PaymentGiro,
                Transfer = x.PaymentTransfer,
                Jenis = int.Parse(x.OrderType.ToString()),
                Nominal = x.OrderNominal,
                IsActive = 1,
                IsDeleted = 0,
                CreatedDt = utcNow,
                CreatedBy = createdBy,
                ReportDate = data.Date.ToUtc(),
                Total = x.PaymentCash + x.PaymentTransfer + x.PaymentGiro
            }
            ).ToList();

            await dbContext.AddRangeAsync(newSalesManualReports);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public DetailUserReportViewModel GetDetailUserReport(string userid, DateTime dateTime)
        {
            try
            {
                dateTime = dateTime.ToUtc();

                var repoUser = unitOfWork.GetRepository<User>();
                repoUser.Condition = PredicateBuilder.True<User>().And(x => x.UserId == userid && x.IsDeleted == 0 && x.IsActive == 1);

                var user = repoUser.Find().FirstOrDefault();
                if (user == null)
                    throw new ApplicationException("not found");
                
                var result = (from a in dbContext.AssignmentGroups
                                join b in dbContext.Users on a.CreatedBy equals b.UserId
                                join c in dbContext.Accounts.Include(x=>x.Role) on b.AccountId equals c.AccountId
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
                                    StartLatitude = a.StartLatitude??0,
                                    EndLatitude = a.EndLatitude??0,
                                    StartLongitude = a.StartLongitude??0,
                                    EndLongitude = a.EndLongitude??0,
                                    StartTime = a.StartTime,
                                    TotalLostTime = a.TotalLostTime,
                                    EndedTime = a.EndTime,
                                    VisitHistory = (from x in dbContext.AssignmentDetails
                                                    join y in dbContext.Assignments on x.AssignmentId equals y.AssignmentId
                                                    join z in dbContext.AssignmentStatuses on y.AssignmentStatusCode equals z.AssignmentStatusCode
                                                    where x.UserId == userid && (x.EndTime != null && x.EndTime.Value.Date == a.StartTime.Date)
                                                    select new VisitHistoryViewModel
                                                    {
                                                        CustomerName = y.AssignmentName,
                                                        EndedTime = x.EndTime,
                                                        StartedTime = x.StartTime,
                                                        Status = z.AssignmentStatusName,
                                                        TaskName = y.AssignmentName,
                                                        TotalInvoice = y.Invoices.Count
                                                    }
                                                    ).ToList(),
                                    LocationHistory = (from h in dbContext.AssignmentDetails.Include(i => i.Assignment).Include(j => j.Contact)
                                                       join i in dbContext.UserActivities on h.AssignmentId equals i.AssignmentId into userActivity
                                                       from i in userActivity.DefaultIfEmpty()
                                                       where h.UserId == b.UserId && h.StartTime.Date == a.StartTime.Date                                                       select new LocationHistoryViewModel
                                                       {
                                                           Label = h.Contact.ContactName + ": " + h.Assignment.Address,
                                                           Latitude = i.Latitude,
                                                           Status = i.AssignmentStatusCode,
                                                           Longitude = i.Longitude,
                                                           StartTime = i.CreatedDt
                                                       }
                                                    ).ToList()
                                }).FirstOrDefault();

                return result;

            }
            catch(ApplicationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> GetManualReportCount(string keyword, DateTime date)
        {
            date = date.ToUtc();

            var totalData = await dbContext.SalesManualReports.Where(x=> (x.ReportDate >= date && x.ReportDate < date.AddDays(1) ) 
            && (x.SalesCode.StartsWith(keyword) 
            || x.SalesName.StartsWith(keyword)
            || x.CustomerCode.StartsWith(keyword)
            || x.CustomerName.StartsWith(keyword)))
            .CountAsync();

            return totalData;
        }

        public async Task<List<DailyReportItemViewModel>> GetManualReport(string keyword, DateTime startDate, int limit = 10, int offset = 0, string orderColumn = "SalesCode", string orderType = "asc")
        {
            startDate = startDate.ToUtc();

            var result = await dbContext.SalesManualReports.Where(x=> (x.ReportDate >= startDate && x.ReportDate < startDate.AddDays(1) ) 
            && (x.SalesCode.StartsWith(keyword) 
            || x.SalesName.StartsWith(keyword)
            || x.CustomerCode.StartsWith(keyword)
            || x.CustomerName.StartsWith(keyword)))
            .Select(x=> new DailyReportItemViewModel {
                UserCode = x.SalesCode,
                UserName = x.SalesName,
                CustomerCode = x.CustomerCode,
                CustomerName = x.CustomerName,
                PaymentCash = x.Cash,
                PaymentTransfer = x.Transfer,
                PaymentGiro = x.Giro,
                OrderType = x.Jenis,
                OrderNominal = x.Nominal,
                Date = x.ReportDate,
                InvoiceValue = x.InvoiceValue
            })
            .Skip(offset)
            .Take(limit)
            .ToListAsync();

            return result;
        }
    }
}
