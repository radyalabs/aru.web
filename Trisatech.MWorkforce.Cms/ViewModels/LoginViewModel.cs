using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Cms.ViewModels
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsRemember { get; set; }
    }
}
