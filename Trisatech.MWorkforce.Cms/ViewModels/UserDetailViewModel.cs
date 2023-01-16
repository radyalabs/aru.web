using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Cms.ViewModels
{
    public class UserDetailViewModel
    {
        //--Account 
        public string AccountId { get; set; }
        public string RoleId { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }

        //--User
        public string UserId { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
    }
}
