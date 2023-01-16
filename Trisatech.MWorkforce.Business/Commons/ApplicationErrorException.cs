using System;
using System.Collections.Generic;
using System.Text;

namespace Trisatech.MWorkforce.Business.Commons
{
    public class ApplicationErrorException : Exception
    {
        public ApplicationErrorException():base("Terjadi kesalahan pada aplikasi")
        {
        }
    }
}
