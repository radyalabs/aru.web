using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Cms.ViewModels
{
    public class JsonResponseViewModel
    {
        public bool success { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
}
