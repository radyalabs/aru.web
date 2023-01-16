using Trisatech.MWorkforce.Business.Entities;
using Trisatech.MWorkforce.Business.Interfaces;
using Trisatech.MWorkforce.Domain;
using Trisatech.MWorkforce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Trisatech.AspNet.Common.Helpers;
using Trisatech.AspNet.Common.Interfaces;
using Trisatech.AspNet.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trisatech.MWorkforce.Business.Services
{
	public class TerritoryService : ITerritoryService
	{
		private IUnitOfWork unitOfWork;
		private IRepository<Territory> repoTerritory;
		private MobileForceContext dbContext;

		public TerritoryService(MobileForceContext dbContext)
		{
			this.dbContext = dbContext;
			unitOfWork = new UnitOfWork<DbContext>(dbContext);
			repoTerritory = unitOfWork.GetRepository<Territory>();
		}

		#region CRUD TERRITORY

		public void Add(TerritoryModel model, string createdBy)
		{
			try
			{
				var repoUserTerritory = unitOfWork.GetRepository<UserTerritory>();

				Territory territory = new Territory();
				CopyProperty.CopyPropertiesTo(model, territory);

				territory.CreatedBy = createdBy;
				territory.CreatedDt = DateTime.UtcNow;
				territory.IsActive = 1;
				territory.IsDeleted = 0;

				UserTerritory userTerritory = new UserTerritory
				{
					TerritoryId = territory.TerritoryId,
					UserId = model.UserId,
				};

				repoUserTerritory.Insert(userTerritory);
				var resp = repoTerritory.Insert(territory, true);
				if (!string.IsNullOrEmpty(resp))
					throw new ApplicationException(resp);
			}
			catch (ApplicationException appEx)
			{
				throw appEx;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void Delete(string id, string deletedBy)
		{
			try
			{
				repoTerritory.Condition = PredicateBuilder.True<Territory>().And(x => x.TerritoryId == id && x.IsActive == 1 && x.IsDeleted == 0);
				var territory = repoTerritory.Find().FirstOrDefault();
				if (territory == null)
					throw new ApplicationException("data tidak ditemukan");
				territory.IsDeleted = 1;
				territory.UpdatedBy = deletedBy;
				territory.UpdatedDt = DateTime.UtcNow;

				var resp = repoTerritory.Update(territory, true);
				if (!string.IsNullOrEmpty(resp))
					throw new ApplicationException(resp);
			}
			catch (ApplicationException appEx)
			{
				throw appEx;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void Edit(TerritoryModel model, string updatedBy)
		{
			try
			{
				repoTerritory.Condition = PredicateBuilder.True<Territory>().And(x => x.TerritoryId == model.TerritoryId && x.IsActive == 1 && x.IsDeleted == 0);
				var territory = repoTerritory.Find().FirstOrDefault();
				if (territory == null)
					throw new ApplicationException("data tidak ditemukan");

				territory.Name = model.Name;
				territory.Desc = model.Desc;
				territory.UpdatedBy = updatedBy;
				territory.UpdatedDt = DateTime.UtcNow;

				var resp = repoTerritory.Update(territory, true);
				if (!string.IsNullOrEmpty(resp))
					throw new ApplicationException(resp);
			}
			catch (ApplicationException appEx)
			{
				throw appEx;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public List<TerritoryModel> Get(string keyword = "", int limit = 20, int offset = 0, string orderColumn = "CreatedDt", string orderType = "desc")
		{
			try
			{
				repoTerritory.Condition = PredicateBuilder.True<Territory>().And(x => x.IsActive == 1 && x.IsDeleted == 0);
				if (!string.IsNullOrEmpty(keyword))
				{
					repoTerritory.Condition.And(x => x.Name.Contains(keyword) || x.Desc.Contains(keyword));
				}

				repoTerritory.OrderBy = new Trisatech.AspNet.Common.Models.SqlOrderBy
				{
					Column = orderColumn,
					Type = orderType
				};

				repoTerritory.Limit = limit;
				repoTerritory.Offset = offset;

				var result = repoTerritory.Find<TerritoryModel>(x => new TerritoryModel
				{
					TerritoryId = x.TerritoryId,
					Name = x.Name,
					Desc = x.Desc
				}).ToList();

				return result;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public TerritoryModel Get(string id)
		{
			try
			{
				var repoTerritory = unitOfWork.GetRepository<Territory>();
				repoTerritory.Condition = PredicateBuilder.True<Territory>().And(x => x.TerritoryId == id && x.IsDeleted == 0);


				return repoTerritory.Find<TerritoryModel>(x => new TerritoryModel
				{
					TerritoryId = x.TerritoryId,
					Name = x.Name,
					Desc = x.Desc,
					UserTerritories = (from a in dbContext.UserTerritories
							  join b in dbContext.Users on a.UserId equals b.UserId
							  join c in dbContext.Territories on a.TerritoryId equals c.TerritoryId
							  where a.TerritoryId == id
							  select new UserTerritoryModel
							  {
								  User = new UserModel
								  {
									  Name = b.UserName,
									  RoleName = b.Account.Role.RoleName,
									  UserId = a.UserId,
								  },
								  TerritoryId = a.TerritoryId,

							  }).ToList(),
				}).FirstOrDefault();

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public int Total(string keyword, int limit = 10, int offset = 0, string orderColumn = "CreatedDt", string orderType = "desc")
		{
			try
			{
				repoTerritory.Condition = PredicateBuilder.True<Territory>().And(x => x.IsActive == 1 && x.IsDeleted == 0);
				if (!string.IsNullOrEmpty(keyword))
				{
					repoTerritory.Condition.And(x => x.Name.Contains(keyword) || x.Desc.Contains(keyword));
				}

				repoTerritory.OrderBy = new Trisatech.AspNet.Common.Models.SqlOrderBy
				{
					Column = "CreatedDt",
					Type = "desc"
				};

				repoTerritory.Limit = limit;
				repoTerritory.Offset = offset;

				return (int)repoTerritory.Count();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		#endregion



	}
}
