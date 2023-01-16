using Trisatech.MWorkforce.Business.Interfaces;
using Trisatech.MWorkforce.Domain;
using Trisatech.MWorkforce.Domain.Entities;
using Trisatech.AspNet.Common.Interfaces;
using Trisatech.AspNet.Common.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Trisatech.MWorkforce.Business.Services
{
    public class AuthenticationService : IAuthenticationService
    {

        private IUnitOfWork unitOfWork;
        private IRepository<Otp> repoOtp;
        private MobileForceContext dbContext;
        public AuthenticationService(MobileForceContext dbContext)
        {
            this.dbContext = dbContext;
            unitOfWork = new UnitOfWork<MobileForceContext>(dbContext);
            repoOtp = this.unitOfWork.GetRepository<Otp>();
        }

        public bool CheckOtp(string value, string itemId = "")
        {
            try
            {
                DateTime now = DateTime.UtcNow;

                repoOtp.Condition = PredicateBuilder.True<Otp>().And(x => x.Value == value);
                if (!string.IsNullOrEmpty(itemId))
                {
                    repoOtp.Condition = repoOtp.Condition.And(x => x.ItemId == itemId);
                }

                var result = repoOtp.Find().FirstOrDefault();
                if (result == null)
                    throw new ApplicationException("invalid otp. try again!");

                if(result.IsValid)
                {
                    if (now > result.ValidTime)
                        return false;
                }

                return result.IsValid;
            }catch(ApplicationException appEx)
            {
                throw appEx;
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public void RequestOtp(string type, string itemId, string value, DateTime validTime, string createdBy)
        {
            try
            {
                Otp otp = new Otp
                {
                    Type = type,
                    Value = value,
                    IsValid = true,
                    CreatedBy = createdBy,
                    CreatedDt = DateTime.UtcNow,
                    ItemId = itemId,
                    ValidTime = validTime
                };

                string resp = repoOtp.Insert(otp, true);
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

        public void SubmitOtp(string value, string itemId = "")
        {
            try
            {
                DateTime now = DateTime.UtcNow;

                repoOtp.Condition = PredicateBuilder.True<Otp>().And(x => x.Value == value);
                if (!string.IsNullOrEmpty(itemId))
                {
                    repoOtp.Condition = repoOtp.Condition.And(x => x.ItemId == itemId);
                }

                var result = repoOtp.Find().FirstOrDefault();
                if (result == null)
                    throw new ApplicationException("not found");

                result.IsValid = true;

                string resp = repoOtp.Update(result, true);
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
