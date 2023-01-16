using Trisatech.MWorkforce.Business.Entities;
using Trisatech.MWorkforce.Business.Interfaces;
using Trisatech.MWorkforce.Domain;
using Trisatech.MWorkforce.Domain.Entities;
using Trisatech.AspNet.Common.Helpers;
using Trisatech.AspNet.Common.Interfaces;
using Trisatech.AspNet.Common.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Business.Services
{
    public class UserActivityService : IUserActivityService
    {
        private IUnitOfWork unitOfWork;
        private IRepository<UserActivity> repoUserActivity;
        private MobileForceContext dbContext;
        public UserActivityService(MobileForceContext context)
        {
            dbContext = context;
            unitOfWork = new UnitOfWork<MobileForceContext>(context);
            repoUserActivity = this.unitOfWork.GetRepository<UserActivity>();
        }

        public void Add(UserActivityModel userAcivity)
        {
            try
            {
                UserActivity activity = new UserActivity();
                CopyProperty.CopyPropertiesTo(userAcivity, activity);

                var resp = repoUserActivity.Insert(activity, true);
                if (!string.IsNullOrEmpty(resp))
                    throw new ApplicationException(resp);
            }catch(ApplicationException appEx)
            {
                throw appEx;
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public Task<List<UserActivityModel>> GetUserActivities()
        {
            throw new NotImplementedException();
        }

        public Task<List<UserActivityModel>> GetUserActivities(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
