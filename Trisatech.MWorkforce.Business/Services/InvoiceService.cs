using Trisatech.MWorkforce.Business.Entities;
using Trisatech.MWorkforce.Business.Interfaces;
using Trisatech.MWorkforce.Domain;
using Trisatech.MWorkforce.Domain.Entities;
using Trisatech.AspNet.Common.Interfaces;
using Trisatech.AspNet.Common.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Trisatech.AspNet.Common.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Trisatech.MWorkforce.Business.Services
{
    public class InvoiceService : IInvoiceService
    {
        private IUnitOfWork unitOfWork;
        private IRepository<Invoice> repoInvoice;
        private MobileForceContext dbContext;
        public InvoiceService(MobileForceContext mobileForceContext)
        {
            dbContext = mobileForceContext;
            unitOfWork = new UnitOfWork<MobileForceContext>(mobileForceContext);
            repoInvoice = this.unitOfWork.GetRepository<Invoice>();
        }
        public List<InvoiceModel> GetInvoices(string assignmentId)
        {
            try
            {
                repoInvoice.Includes = new string[] { "Assignment" };
                repoInvoice.Condition = PredicateBuilder.True<Invoice>().And(x => x.IsActive == 1 && x.IsDeleted == 0 && x.AssignmentId == assignmentId && x.Assignment.IsDeleted == 0 && x.Assignment.IsActive == 1);

                var invoices = repoInvoice.Find<InvoiceModel>(x => new InvoiceModel
                {
                    InvoiceId = x.InvoiceId,
                    Amount = x.Amount,
                    AssignmentCode = x.AssignmentCode,
                    InvoiceCode = x.InvoiceCode,
                    Status = x.Status,
                    Type = (x.Amount > 0 ? (short)1:(short)0)
                }).ToList();
                
                return invoices;
            }catch(ApplicationException appEx)
            {
                throw appEx;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        public void SubmitPayment(List<PaymentModel> paymentModel, string createdBy, string assignmentId = "", bool isVerified = false)
        {
            try
            {
                unitOfWork.Strategy().Execute(() =>
                {
                    unitOfWork.BeginTransaction();

                    var repoPayment = unitOfWork.GetRepository<Payment>();
                    DateTime utcNow = DateTime.UtcNow;

                    foreach (var item in paymentModel)
                    {
                        Payment payment = new Payment();
                        CopyProperty.CopyPropertiesTo(item, payment);

                        payment.CreatedBy = createdBy;
                        payment.CreatedDt = utcNow;
                        payment.IsActive = 1;
                        payment.IsDeleted = 0;

                        var resp = repoPayment.Insert(payment);
                        if (!string.IsNullOrEmpty(resp))
                            throw new ApplicationException(resp);
                    }

                    if (!string.IsNullOrEmpty(assignmentId))
                    {
                        UpdateAssignmentDetail(assignmentId, isVerified);
                    }

                    unitOfWork.Commit();
                });
            }
            catch (ApplicationException appEx)
            {
                unitOfWork.Rollback(); 
                throw appEx;
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                throw ex;
            }
        }
        private void UpdateAssignmentDetail(string assignmentId, bool isVerified)
        {
            var repoAssignmentDetail = unitOfWork.GetRepository<AssignmentDetail>();

            repoAssignmentDetail.Condition = PredicateBuilder.True<AssignmentDetail>().And(a => a.AssignmentId == assignmentId);

            var assignment = repoAssignmentDetail.Find().FirstOrDefault();

            if (assignment != null)
            {
                assignment.IsVerified = isVerified;
                assignment.UpdatedDt = DateTime.UtcNow;

                repoAssignmentDetail.Update(assignment);
            }
        }

        public void SubmitPayment(PaymentModel paymentModel, string createdBy)
        {
            try
            {
                var repoPayment = unitOfWork.GetRepository<Payment>();

                Payment payment = new Payment();
                CopyProperty.CopyPropertiesTo(paymentModel, payment);

                payment.CreatedBy = createdBy;
                payment.CreatedDt = DateTime.UtcNow;
                payment.IsActive = 1;
                payment.IsDeleted = 0;

                var resp = repoPayment.Insert(payment, true);
                if (!string.IsNullOrEmpty(resp))
                    throw new ApplicationException(resp);

            }
            catch (ApplicationException appEx)
            {
                throw appEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
