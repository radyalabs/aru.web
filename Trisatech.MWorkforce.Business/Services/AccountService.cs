using Trisatech.MWorkforce.Business.Entities;
using Trisatech.MWorkforce.Business.Interfaces;
using Trisatech.MWorkforce.Domain;
using Trisatech.MWorkforce.Domain.Entities;
using Trisatech.AspNet.Common.Interfaces;
using Trisatech.AspNet.Common.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Trisatech.MWorkforce.Business.Resources;
using Microsoft.EntityFrameworkCore;

namespace Trisatech.MWorkforce.Business.Services
{
    public class AccountService : IAccountService
    {
        private IUnitOfWork unitOfWork;
        private IRepository<User> repoUser;
        public AccountService(MobileForceContext context)
        {
            unitOfWork = new UnitOfWork<MobileForceContext>(context);
            repoUser = this.unitOfWork.GetRepository<User>();
        }

        //Attendance
        #region Assignment Group
        public AttendanceStatus Check(string userid, DateTime dateTime)
        {
            try
            {
                var repoAssignGroup = unitOfWork.GetRepository<AssignmentGroup>();
                repoAssignGroup.Condition = PredicateBuilder.True<AssignmentGroup>()
                .And(x => x.CreatedBy == userid && 
                (x.StartTime >= dateTime.Date && x.StartTime < dateTime.Date.AddDays(1)));
                var result = repoAssignGroup.Find().FirstOrDefault();

                if (result == null)
                    return AttendanceStatus.ABSENT;
                if (result.EndTime == null)
                    return AttendanceStatus.CHECKIN;
                else
                    return AttendanceStatus.CHECKOUT;
            }
            catch { throw; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="startTime"></param>
        /// <param name="reserved1"></param>
        /// <param name="reserved3">Sales ID (SPV Checkin)</param>
        /// <param name="startDistance"></param>
        public void CheckIn(string userId, double latitude, double longitude, DateTime startTime, string reserved1 = "", string reserved3 = "", double startDistance = 0, string reserved4 = "", string reserved5 = "")
        {
            try
            {
                var repoAssignGroup = unitOfWork.GetRepository<AssignmentGroup>();

                AssignmentGroup assignmentGroup = new AssignmentGroup
                {
                    AssignmentGroupId = Guid.NewGuid().ToString(),
                    CreatedBy = userId,
                    CreatedDt = DateTime.UtcNow,
                    StartTime = startTime,
                    StartLatitude = latitude,
                    StartLongitude = longitude,
                    StartDistance = startDistance,
                    TotalLostTime = 0,
                    Reserved1 = reserved1,
                    Reserved3 = reserved3
                };

                if(!string.IsNullOrEmpty(reserved4) && !string.IsNullOrEmpty(reserved5))
                {
                    assignmentGroup.Reserved4 = reserved4;
                    assignmentGroup.Reserved5 = reserved5;
                }

                string resp = repoAssignGroup.Insert(assignmentGroup, true);
                if (!string.IsNullOrEmpty(resp))
                    throw new ApplicationException(resp);

            }catch(ApplicationException appEx)
            {
                throw appEx;
            }
            catch { throw; }
        }

        public void CheckOut(string userId, double latitude, double longitude, DateTime endTime, string reserved1 = "", string reserved3 = "", double endDistance = 0, string reserved6 = "", string reserved7 = "")
        {
            try
            {
                var repoAssignGroup = unitOfWork.GetRepository<AssignmentGroup>();
                repoAssignGroup.Condition = PredicateBuilder.True<AssignmentGroup>().And(x => x.CreatedBy == userId && x.EndTime == null);
                repoAssignGroup.OrderBy = new Trisatech.AspNet.Common.Models.SqlOrderBy { Column = "CreatedDt", Type = "desc" };

                var assignmentGroup = repoAssignGroup.Find().FirstOrDefault();

                if (assignmentGroup == null)
                    throw new ApplicationException("not yet checkin");
                assignmentGroup.EndTime = endTime;
                assignmentGroup.EndDistance = endDistance;
                assignmentGroup.EndLatitude = latitude;
                assignmentGroup.EndLongitude = longitude;
                assignmentGroup.Reserved2 = reserved1;

                if (!string.IsNullOrEmpty(reserved6) && !string.IsNullOrEmpty(reserved7))
                {
                    assignmentGroup.Reserved6 = reserved6;
                    assignmentGroup.Reserved7 = reserved7;
                }
                
                string resp = repoAssignGroup.Update(assignmentGroup, true);
                if (!string.IsNullOrEmpty(resp))
                    throw new ApplicationException(resp);
            }
            catch (ApplicationException appEx)
            {
                throw appEx;
            }
            catch { throw; }
        }
        
        public string GetCheckinPartner(string userId, DateTime dateTime)
        {
            try
            {
                var repoAssignGroup = unitOfWork.GetRepository<AssignmentGroup>();
                repoAssignGroup.Condition = PredicateBuilder.True<AssignmentGroup>()
                .And(x => x.CreatedBy == userId 
                && (x.StartTime >= dateTime && x.StartTime < dateTime.AddDays(1)));
                var result = repoAssignGroup.Find().FirstOrDefault();

                if (result == null)
                    return "";
                else
                    return result.Reserved3;
            }
            catch{ throw; }
        }
        
        public AssignmentGroup Get(string userid, DateTime date)
        {
            try
            {
                var repoAssignGroup = unitOfWork.GetRepository<AssignmentGroup>();
                repoAssignGroup.Condition = PredicateBuilder.True<AssignmentGroup>().And(x => x.CreatedBy == userid && x.StartTime.Date == date.Date);
                var result = repoAssignGroup.Find().FirstOrDefault();

                return result;
            }
            catch { throw; }
        }

        public UserActivityModel GetUserActivity(string userid, DateTime date)
        {
            try
            {
                var context = repoUser.GetDbContext() as  MobileForceContext;
                var repoUserActivity = unitOfWork.GetRepository<UserActivity>();

                repoUserActivity.Condition = PredicateBuilder.True<UserActivity>().And(x => x.CreatedBy == userid 
                && x.CreatedDt.Date == date.Date 
                && x.ActivityTypeEnum == UserActivityEnum.COMPLETE_ASSIGNMENT);

                repoUserActivity.OrderBy = new Trisatech.AspNet.Common.Models.SqlOrderBy { Column = "CreatedDt", Type = "desc" };
                repoUserActivity.Limit = 1;
                repoUserActivity.Offset = 0;

                var result = repoUserActivity.Find<UserActivityModel>(x => new UserActivityModel
                {
                    ActivityTypeEnum = x.ActivityTypeEnum,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    UserActivityId = x.UserActivityId,
                    CreatedDt = x.CreatedDt,
                    CreatedBy = x.CreatedBy
                }).FirstOrDefault();

                return result;
            }
            catch { throw; }
        }
        #endregion


        public List<UserAgentModel> GetUserAgent(string role, List<string> territoryId = null)
        {
            try
            {
                if(territoryId != null && territoryId.Count > 0)
                {
                    var repoUserTerritory = unitOfWork.GetRepository<UserTerritory>();
                    repoUserTerritory.Includes = new string[] { "User", "User.Account", "User.Account.Role" };
                    repoUserTerritory.Condition = PredicateBuilder.True<UserTerritory>().And(x => territoryId.Contains(x.TerritoryId) && (x.User.Account.RoleCode == role && x.User.IsDeleted == 0));

                    var users = repoUserTerritory.Find<UserAgentModel>(x => new UserAgentModel
                    {
                        UserCode = x.User.UserCode,
                        UserId = x.User.UserId,
                        UserName = x.User.UserName,
                        UserPhone = x.User.UserPhone
                    }).ToList();

                    return users;
                }
                else
                {
                    repoUser.Condition = PredicateBuilder.True<User>().And(x => x.IsActive == 1 && x.IsDeleted == 0);
                    repoUser.Includes = new string[] { "Account", "Account.Role" };
                    repoUser.Condition = repoUser.Condition.And(x => x.Account.RoleCode == role);

                    var users = repoUser.Find<UserAgentModel>(x => new UserAgentModel
                    {
                        UserId = x.UserId,
                        UserCode = x.UserCode,
                        UserName = x.UserName,
                        UserPhone = x.UserPhone
                    }).ToList();

                    return users;
                }
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
            catch { throw; }
        }

        public UserAuthenticated Login(string username, string password)
        {
            try
            {
                repoUser.Condition = PredicateBuilder.True<User>().And(x => x.IsActive == 1 && x.IsDeleted == 0);
                repoUser.Includes = new string[] { "Account", "Account.Role", "UserTerritory" };
                
                repoUser.Condition = repoUser.Condition.And(x => x.UserName.Equals(username) && x.Account.Password.Equals(password));

                var user = repoUser.Find().FirstOrDefault();
                if (user == null)
                    throw new ApplicationException(GlobalAppMessage.AuthenticationFailed);

                /*
                if (!string.IsNullOrEmpty(user.Account.CurrentToken))
                {
                    throw new ApplicationException("Other user sign in with this credential. Please logout first.");
                }
                */

                var userAuth = new UserAuthenticated(user.Account.AccountId, user.UserId, user.UserCode, user.Account.RoleCode, user.Account.Role.RoleName, user.UserName, user.UserTerritory.Where(x=> !x.TerritoryId.Trim().Equals(("Please select one").Trim())).Select(x=>x.TerritoryId).ToList());

                return userAuth;
            }catch(ApplicationException ex)
            {
                throw ex;
            }
            catch
            {
                throw;
            }
        }

        public void Logout(string token)
        {
            try
            {
                var repoAccount = unitOfWork.GetRepository<Account>();
                repoAccount.Condition = PredicateBuilder.True<Account>().And(x => x.CurrentToken == token && x.IsActive == 1 && x.IsDeleted == 0);

                var account = repoAccount.Find().FirstOrDefault();
                if (account == null)
                    throw new ApplicationException("not found");

                account.CurrentToken = string.Empty;
                account.UpdatedDt = DateTime.UtcNow;

                var resp = repoAccount.Update(account, true);
                if (!string.IsNullOrEmpty(resp))
                    throw new ApplicationException(resp);

            }catch(ApplicationException appEx)
            {
                throw appEx;
            }catch
            {
                throw;
            }
        }

        public void SetSession(string userid, string token)
        {
            try
            {
                repoUser.Condition = PredicateBuilder.True<User>().And(x => x.IsActive == 1 && x.IsDeleted == 0);
                repoUser.Includes = new string[] { "Account" };

                repoUser.Condition = repoUser.Condition.And(x => x.UserId == userid);

                var user = repoUser.Find().FirstOrDefault();
                if (user == null)
                    throw new ApplicationException(GlobalAppMessage.AuthenticationFailed);

                var repoAccount = unitOfWork.GetRepository<Account>();
                var repoUserLogin = unitOfWork.GetRepository<UserLogin>();

                unitOfWork.Strategy().Execute(() =>
                {
                    unitOfWork.BeginTransaction();

                    var account = user.Account;

                    account.CurrentToken = token;
                    account.LastLoginDt = DateTime.UtcNow;
                    account.UpdatedBy = userid;
                    account.UpdatedDt = DateTime.UtcNow;

                    //update account
                    string respMessage = repoAccount.Update(account);
                    if (!string.IsNullOrEmpty(respMessage))
                        throw new ApplicationException(respMessage);

                    UserLogin userLogin = new UserLogin()
                    {
                        AccountId = account.AccountId,
                        GeneratedToken = token,
                        CreatedBy = userid,
                        CreatedDt = DateTime.UtcNow
                    };

                    //Insert user login
                    respMessage = repoUserLogin.Insert(userLogin);
                    if (!string.IsNullOrEmpty(respMessage))
                        throw new ApplicationException(respMessage);

                    unitOfWork.Commit();
                });

            }
            catch (ApplicationException appEx)
            {
                unitOfWork.Rollback();
                throw appEx;
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }
        
        public bool Validate(string userid, string password)
        {
            throw new NotImplementedException();
        }

        public void UpdateAssignmentGroup(string assignmentGroupId, int duration)
        {
            try
            {
                var repoAssignGroup = unitOfWork.GetRepository<AssignmentGroup>();
                repoAssignGroup.Condition = PredicateBuilder.True<AssignmentGroup>().And(x => x.AssignmentGroupId == assignmentGroupId);
                var result = repoAssignGroup.Find().FirstOrDefault();

                result.TotalLostTime += duration;
                var resp = repoAssignGroup.Update(result, true);
                if (!string.IsNullOrEmpty(resp))
                    throw new ApplicationException(resp);
            }catch(ApplicationException appEx)
            {
                throw appEx;
            }
            catch
            {
                throw;
            }
        }
    }
}
