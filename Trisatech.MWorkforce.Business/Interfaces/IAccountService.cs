using Trisatech.MWorkforce.Business.Entities;
using Trisatech.MWorkforce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Trisatech.MWorkforce.Business.Interfaces
{
    public interface IAccountService
    {
        UserAuthenticated Login(string username, string password);
        void SetSession(string userid, string token);
        bool Validate(string userid, string password);
        void Logout(string token);
        List<UserAgentModel> GetUserAgent(string role, List<string> territoryId = null);
        AssignmentGroup Get(string userid, DateTime date);
        UserActivityModel GetUserActivity(string userid, DateTime date);
        AttendanceStatus Check(string userid, DateTime dateTime);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="startTime"></param>
        /// <param name="reserved1"></param>
        /// <param name="reserved3"></param>
        /// <param name="startDistance"></param>
        /// <param name="reserved4"></param>
        /// <param name="reserved5"></param>
        void CheckIn(string userId, double latitude, double longitude, DateTime startTime, string reserved1 = "", string reserved3 = "", double startDistance = 0, string reserved4 = "", string reserved5 = "");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="endTime"></param>
        /// <param name="reserved1"></param>
        /// <param name="reserved3"></param>
        /// <param name="endDistance"></param>
        /// <param name="reserved6"></param>
        /// <param name="reserved7"></param>
        void CheckOut(string userId, double latitude, double longitude, DateTime endTime, string reserved1 = "", string reserved3 = "", double endDistance = 0, string reserved6 = "", string reserved7 = "");
        string GetCheckinPartner(string userId, DateTime dateTime);
        void UpdateAssignmentGroup(string assignmentGroupId, int duration);
    }
}
