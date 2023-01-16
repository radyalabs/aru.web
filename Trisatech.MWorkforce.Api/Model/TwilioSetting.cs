using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Api.Model
{
    public class TwilioSetting
    {
        public TwilioSetting()
        {
            Enable = false;
        }
        public bool Enable { get; set; }
        public string AccountSid { get; set; }
        public string AuthToken { get; set; }
        public string Sender { get; set; }
    }
}
