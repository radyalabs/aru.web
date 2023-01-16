using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace Trisatech.AspNet.Common.Models
{
    public class UserClaimIdentity : IIdentity
    {
        private string _Name { get; set; }
        private string _AuthenticationType { get; set; }

        public UserClaimIdentity(string name, string authName)
        {
            _Name = name;
            _AuthenticationType = authName;
        }
        public string AuthenticationType => _AuthenticationType;

        public bool IsAuthenticated => true;

        public string Name => _Name;
    }
}
