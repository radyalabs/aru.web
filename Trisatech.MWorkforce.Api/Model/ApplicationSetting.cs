using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Api.Model
{
    public class ApplicationSetting
    {
        public string EncryptionKey { get; set; }
        public string VerificationKey { get; set; }
        public string AuthHeaderKey { get; set; }
    }
}
