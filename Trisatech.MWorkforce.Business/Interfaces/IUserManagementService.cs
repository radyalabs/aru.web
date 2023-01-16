using Trisatech.MWorkforce.Business.Entities;
using Trisatech.MWorkforce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Trisatech.MWorkforce.Business.Interfaces
{
    public interface IUserManagementService
    {
        void Add(UserModel userModel, string createdBy, bool withTerritory = false);
        void Edit(UserModel userModel, string updatedBy, bool isPassChanged);
        void Delete(string userId, string deletedBy);
        List<UserModel> Users(string keyword = "", int limit = 10, int offset = 0, string orderColumn = "CreatedDt", string orderType = "desc");
        Task<List<UserModel>> GetUserByRoles(params string[] roleCode);
        List<Role> GetRoles();
        UserModel DetailUser(string id);
        long TotalUsers(string search = "", int length = 10, int start = 0, string v = "CreatedDt", string orderType = "desc");
    }
}
