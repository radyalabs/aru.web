using Trisatech.MWorkforce.Business.Entities;
using Trisatech.MWorkforce.Business.Interfaces;
using Trisatech.MWorkforce.Domain;
using Trisatech.MWorkforce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Trisatech.AspNet.Common.Interfaces;
using Trisatech.AspNet.Common.Repository;
namespace Trisatech.MWorkforce.Business.Services
{
    public class UserManagementService : IUserManagementService
    {

        private IUnitOfWork unitOfWork;
        private IRepository<User> repoUser;
        private readonly MobileForceContext dbContext;
        public UserManagementService(MobileForceContext mobileForceContext)
        {
            dbContext = mobileForceContext;
            unitOfWork = new UnitOfWork<MobileForceContext>(dbContext);
            repoUser = unitOfWork.GetRepository<User>();
        }

        public void Add(UserModel userModel, string createdBy, bool withTerritory = false)
        {
            var repoAccount = unitOfWork.GetRepository<Account>();
            try
            {
                var repoUserTerritory = unitOfWork.GetRepository<UserTerritory>();

                unitOfWork.Strategy().Execute(() =>
                {
                    unitOfWork.BeginTransaction();
                    DateTime utcNow = DateTime.UtcNow;

                    Account newAccount = new Account()
                    {
                        AccountId = userModel.AccountId,
                        RoleId = userModel.RoleId,
                        RoleCode = userModel.RoleCode,
                        Password = userModel.Password,
                        CreatedBy = createdBy,
                        CreatedDt = utcNow,
                        IsActive = 1,
                        IsDeleted = 0
                    };


                    User newUser = new User
                    {
                        UserId = userModel.UserId,
                        AccountId = userModel.AccountId,
                        UserCode = userModel.UserCode,
                        FullName = userModel.Name,
                        UserEmail = userModel.UserEmail,
                        UserPhone = userModel.UserPhone,
                        UserName = userModel.UserName,
                        CreatedBy = createdBy,
                        CreatedDt = utcNow,
                        IsActive = 1,
                        IsDeleted = 0
                    };

                    if (withTerritory && userModel.Territoryid != null)
                    {
                        UserTerritory userTerritory = new UserTerritory
                        {
                            UserId = userModel.UserId,
                            TerritoryId = userModel.Territoryid
                        };

                        repoUserTerritory.Insert(userTerritory);
                    }

                    string resp = repoAccount.Insert(newAccount);
                    if (!string.IsNullOrEmpty(resp))
                        throw new ApplicationException(resp);

                    resp = repoUser.Insert(newUser);
                    if (!string.IsNullOrEmpty(resp))
                        throw new ApplicationException(resp);

                    unitOfWork.Commit();
                });

            }catch(ApplicationException appEx){
                unitOfWork.Rollback();
                throw appEx;
            }catch(Exception ex)
            {
                unitOfWork.Rollback();
                throw ex;
            }
        }

        public void Delete(string userId, string deletedBy)
        {
            try
            {
                var repoAccount = unitOfWork.GetRepository<Account>();
                repoUser.Includes = new string[] { "Account", "Account.Role" };
                repoUser.Condition = PredicateBuilder.True<User>().And(x => x.IsActive == 1 && x.IsDeleted == 0 && x.UserId == userId);

                var user = repoUser.Find().FirstOrDefault();
                if (user == null && user.Account == null)
                    throw new ApplicationException("not found");

                unitOfWork.Strategy().Execute(()=>
                {
                    unitOfWork.BeginTransaction();
                    DateTime utcNow = DateTime.UtcNow;

                    user.Account.IsActive = 0;
                    user.Account.IsDeleted = 1;
                    user.Account.UpdatedBy = deletedBy;
                    user.Account.UpdatedDt = utcNow;

                    string resp = repoAccount.Update(user.Account);
                    if (!string.IsNullOrEmpty(resp))
                        throw new ApplicationException(resp);

                    user.IsDeleted = 1;
                    user.IsActive = 0;
                    user.UpdatedBy = deletedBy;
                    user.UpdatedDt = utcNow;

                    resp = repoUser.Update(user);
                    if (!string.IsNullOrEmpty(resp))
                        throw new ApplicationException(resp);

                    unitOfWork.Commit();
                });
            }
            catch (ApplicationException appEx)
            {
                unitOfWork.Rollback();
                throw appEx;
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                throw ex;
            }
        }

        public void Edit(UserModel userModel, string updatedBy, bool isPassChanged)
        {
            try
            {
				var repoUserTerritory = unitOfWork.GetRepository<UserTerritory>();
				var repoAccount = unitOfWork.GetRepository<Account>();
                repoUser.Includes = new string[] { "Account", "Account.Role" };
                repoUser.Condition = PredicateBuilder.True<User>().And(x => x.IsActive == 1 && x.IsDeleted == 0 && x.UserId == userModel.UserId);

                var user = repoUser.Find().FirstOrDefault();
                if (user == null && user.Account == null)
                    throw new ApplicationException("not found");

                unitOfWork.Strategy().Execute(()=>
                {
                    unitOfWork.BeginTransaction();
                    DateTime utcNow = DateTime.UtcNow;

                    user.Account.RoleId = userModel.RoleId;
                    user.Account.RoleCode = userModel.RoleCode;
                    user.Account.UpdatedBy = updatedBy;
                    user.Account.UpdatedDt = utcNow;
                    if (isPassChanged)
                        user.Account.Password = userModel.Password;

                    string resp = repoAccount.Update(user.Account);
                    if (!string.IsNullOrEmpty(resp))
                        throw new ApplicationException(resp);

                    user.FullName = userModel.Name;
                    user.UserCode = userModel.UserCode;
                    user.UserEmail = userModel.UserEmail;
                    user.UserName = userModel.UserName;
                    user.UserPhone = userModel.UserPhone;
                    user.UpdatedBy = updatedBy;
                    user.UpdatedDt = utcNow;



                    resp = repoUser.Update(user);

                    if (!string.IsNullOrEmpty(userModel.Territoryid))
                    {
                        var userTerritoryExist = (from a in dbContext.UserTerritories
                                                  where a.UserId == user.UserId && a.TerritoryId == userModel.Territoryid
                                                  select a
                                                  ).FirstOrDefault();

                        if(userTerritoryExist == null)
                        {
                            UserTerritory userTerritory = new UserTerritory
                        {
                            UserId = userModel.UserId,
                            TerritoryId = userModel.Territoryid
                        };

                        repoUserTerritory.Insert(userTerritory);
                        if (!string.IsNullOrEmpty(resp))
                            throw new ApplicationException(resp);
                        }
                    }
                    
                    unitOfWork.Commit();
                });
            }
            catch (ApplicationException appEx)
            {
                unitOfWork.Rollback();
                throw appEx;
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                throw ex;
            }
        }

        public List<UserModel> Users(string keyword = "", int limit = 10, int offset = 0, string orderColumn = "", string orderType = "desc")
        {
            try
            {
                repoUser.Includes = new string[] { "Account", "Account.Role" };
                repoUser.Condition = PredicateBuilder.True<User>().And(x => x.IsActive == 1 && x.IsDeleted == 0);
                repoUser.Limit = limit;
                repoUser.Offset = offset;

				repoUser.OrderBy = null;
                if (!string.IsNullOrEmpty(keyword))
                {
                    repoUser.Condition = repoUser.Condition.And(x => x.UserName.Contains(keyword) || x.Account.Role.RoleName.Contains(keyword) || (x.UserEmail != null && x.UserEmail.Contains(keyword)));
                }

                var result = repoUser.Find<UserModel>(x => new UserModel
                {
                    UserEmail = x.UserEmail,
                    AccountId = x.AccountId,
                    RoleCode = x.Account.RoleCode,
                    UserCode = x.UserCode,
                    UserName = x.UserName,
                    Name = x.FullName,
                    UserPhone = x.UserPhone,
                    UserId = x.UserId,
                    RoleName = x.Account.Role.RoleName
                }).ToList();

                return result;
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public List<Role> GetRoles()
        {
            var repoRole = unitOfWork.GetRepository<Role>();
            try
            {
                var result = repoRole.Find();
                return result;
            }catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                repoRole = null;
            }
        }

        public UserModel DetailUser(string id)
        {
			repoUser.Includes = new string[] { "Account", "Account.Role", "UserTerritory", "UserTerritory.Territory" };
            repoUser.Condition = PredicateBuilder.True<User>().And(x => x.IsActive == 1 && x.IsDeleted == 0 && x.UserId == id);
            
            var result = repoUser.Find<UserModel>(x => new UserModel
            {
                UserEmail = x.UserEmail,
                AccountId = x.AccountId,
                RoleCode = x.Account.RoleCode,
                UserCode = x.UserCode,
                Name = x.FullName,
                UserName = x.UserName,
                UserPhone = x.UserPhone,
                UserId = x.UserId,
                RoleId = x.Account.RoleId,
                RoleName = x.Account.Role.RoleName,
				UserTerritories = x.UserTerritory?.Select(y=> new UserTerritoryModel
                {
                    TerritoryId = y.TerritoryId,
                    TerritoryName = y.Territory.Name,
                }).ToList()
			}).FirstOrDefault();

            return result;
        }
        
        public long TotalUsers(string search = "", int length = 10, int start = 1, string v = "", string orderType = "")
        {
            try
            {
                repoUser.Includes = new string[] { "Account", "Account.Role" };
                repoUser.Condition = PredicateBuilder.True<User>().And(x => x.IsActive == 1 && x.IsDeleted == 0);
                repoUser.OrderBy = new Trisatech.AspNet.Common.Models.SqlOrderBy()
                {
                    Column = v,
                    Type = orderType
                };

                if (!string.IsNullOrEmpty(search))
                {
                    repoUser.Condition = repoUser.Condition.And(x => x.UserName.Contains(search) || x.Account.Role.RoleName.Contains(search) || (x.UserEmail != null && x.UserEmail.Contains(search)));
                }

                var result = repoUser.Count();

                return result??0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<List<UserModel>> GetUserByRoles(params string[] roleCode)
        {
            if(roleCode == null || roleCode.Length == 0)
            {
                throw new ArgumentNullException("Role Code cannot be null or empty");
            }
            try
            {
                var result = await (from a in dbContext.Accounts
                                    join b in dbContext.Roles on a.RoleId equals b.RoleId
                                    join c in dbContext.Users on a.AccountId equals c.AccountId
                                    where roleCode.Any(x=> x == b.RoleCode)
                                    select new UserModel{
                                        UserEmail = c.UserEmail,
                                        AccountId = a.AccountId,
                                        RoleCode = b.RoleCode,
                                        UserCode = c.UserCode,
                                        UserName = c.UserName,
                                        Name = c.FullName,
                                        UserPhone = c.UserPhone,
                                        UserId = c.UserId,
                                        RoleName = b.RoleName
                                    }).ToListAsync();
                return result;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
