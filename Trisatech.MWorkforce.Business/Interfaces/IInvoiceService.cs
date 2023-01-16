using Trisatech.MWorkforce.Business.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Trisatech.MWorkforce.Business.Interfaces
{
    public interface IInvoiceService
    {
        void SubmitPayment(PaymentModel paymentModel, string createdBy);
        void SubmitPayment(List<PaymentModel> paymentModel, string createdBy, string assignmentId, bool isVerified = false);
        List<InvoiceModel> GetInvoices(string assignmentId);
    }
}
