using Trisatech.MWorkforce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Cms.ViewModels
{
    public class DataReference
    {
        public static Dictionary<PaymentChannel, string> PaymentChannelDic = new Dictionary<PaymentChannel, string>
        {
            {PaymentChannel.Cash, "Cash" },
            {PaymentChannel.CashAndGiro, "Cash & Giro" },
            {PaymentChannel.CashAndTransfer, "Cash & Transfer" },
            {PaymentChannel.CashAndTransferAndGiro, "Cash, Transfer & Giro" },
            {PaymentChannel.Giro, "Giro" },
            {PaymentChannel.Transfer, "Transfer" },
            {PaymentChannel.TransferAndGiro, "Transfer & Giro" }
        };
    }
}
