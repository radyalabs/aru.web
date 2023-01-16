using System;
using System.Collections.Generic;
using System.Text;

namespace Trisatech.MWorkforce.Infrastructure.Models
{
    public enum SmsStatus : short
    {

        Queued,
        Sending,
        Sent,
        Failed,
        Delivered,
        Undelivered,
        Receiving,
        Received,
        Accepted,
    }
}
