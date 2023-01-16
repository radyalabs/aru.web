using Trisatech.MWorkforce.Business.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Business.Interfaces
{
    public interface IUserActivityService
    {
        void Add(UserActivityModel userAcivity);
        Task<List<UserActivityModel>> GetUserActivities();
        Task<List<UserActivityModel>> GetUserActivities(string userId);
    }
}
