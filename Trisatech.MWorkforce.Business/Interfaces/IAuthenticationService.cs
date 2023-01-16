using System;
using System.Collections.Generic;
using System.Text;

namespace Trisatech.MWorkforce.Business.Interfaces
{
    public interface IAuthenticationService
    {
        void RequestOtp(string type, string itemId, string value, DateTime validTime, string createdBy);
        bool CheckOtp(string value, string itemId = "");
        void SubmitOtp(string value, string itemId = "");
    }
}
