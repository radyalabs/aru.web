using Trisatech.MWorkforce.Domain;
using Trisatech.MWorkforce.Domain.Entities;
using Trisatech.MWorkforce.Cms.Interfaces;
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
    public class DashboardService : IDashboardService
    {

        private IUnitOfWork unitOfWork;
        private MobileForceContext dbContext;

        public DashboardService(MobileForceContext mobileForceContext)
        {
            dbContext = mobileForceContext;
            unitOfWork = new UnitOfWork<MobileForceContext>(dbContext);
        }
        public async Task<List<LocationHistoryViewModel>> GetLocationAgent(string[] regions)
        {
            try
            {
                var result = await (from a in dbContext.Users
                              join b in dbContext.Accounts.Include(x=>x.Role) on a.AccountId equals b.AccountId
                              join c in dbContext.UserTerritories on a.UserId equals c.UserId
                              where a.IsActive == 1 && a.IsDeleted == 0 && b.RoleCode != "SA"
                              && regions.Any(x=>x == c.TerritoryId)
                              select new LocationHistoryViewModel
                              {
                                  Label = a.UserName + "("+ b.Role.RoleName + ")",
                                  Latitude = (from c in dbContext.AssignmentGroups
                                              where a.UserId == c.CreatedBy
                                              orderby c.CreatedDt descending
                                              select c.StartLatitude).FirstOrDefault(),
                                  Longitude = (from d in dbContext.AssignmentGroups
                                               where a.UserId == d.CreatedBy
                                               orderby d.CreatedDt descending
                                               select d.StartLongitude).FirstOrDefault(),
                                  StartTime = (from e in dbContext.AssignmentGroups
                                               where a.UserId == e.CreatedBy
                                               orderby e.CreatedDt descending
                                               select e.StartTime).FirstOrDefault()
                              }).ToListAsync();

                return result;

            }catch(ApplicationException appEx)
            {
                throw appEx;
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CountTotalTask(string[] regions, DateTime date)
        {
            int count = 0;
            try
            {
                var result = await (from a in dbContext.Assignments
                                    join b in dbContext.AssignmentDetails on a.AssignmentId equals b.AssignmentId
                                    join c in dbContext.Users on b.UserId equals c.UserId
                                    join d in dbContext.UserTerritories on c.UserId equals d.UserId
                              where a.AssignmentDate.Date == date.Date
                              && regions.Any(x=> x == d.TerritoryId)
                              select a.AssignmentDate).CountAsync();
                count = result;
                return count;
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<decimal> CountTotalInvoice(string[] regions, DateTime date)
        {
            decimal count = 0;
            try
            {
                var result = await (from a in dbContext.Assignments
                                    join b in dbContext.Invoices on a.AssignmentId equals b.AssignmentId
                                    join c in dbContext.AssignmentDetails on a.AssignmentId equals c.AssignmentId
                                    join d in dbContext.Users on c.UserId equals d.UserId
                                    join e in dbContext.UserTerritories on d.UserId equals e.UserId
                                    where a.AssignmentDate.Date == date.Date
                                    && regions.Any(x => x == e.TerritoryId)
                                    select b.Amount).SumAsync();
                count = result;
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<decimal> CountTotalPayment(string[] regions, DateTime date)
        {
            decimal count = 0;
            try
            {
                var result = await (from a in dbContext.Assignments
                                    join b in dbContext.Payments on a.AssignmentId equals b.AssignmentId
                                    join c in dbContext.AssignmentDetails on a.AssignmentId equals c.AssignmentId
                                    join d in dbContext.Users on c.UserId equals d.UserId
                                    join e in dbContext.UserTerritories on d.UserId equals e.UserId
                                    where a.AssignmentDate.Date == date.Date
                                    && regions.Any(x => x == e.TerritoryId)
                                    where a.AssignmentDate.Date == date.Date

                              select b.PaymentAmount).SumAsync();
                count = result;
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CountTaskFailed (string[] regions, DateTime date)
        {
            try
            {
                var result = await (from a in dbContext.Assignments where a.AssignmentCode == Trisatech.MWorkforce.Business.Entities.ReportConstant.TASK_FAILED
                                    join b in dbContext.AssignmentDetails on a.AssignmentId equals b.AssignmentId
                                    join c in dbContext.Users on b.UserId equals c.UserId
                                    join d in dbContext.UserTerritories on c.UserId equals d.UserId
                                    where a.AssignmentDate.Date == date.Date
                                    && regions.Any(x => x == d.TerritoryId)
                              select a).CountAsync();
                
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
