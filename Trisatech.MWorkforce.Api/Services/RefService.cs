using Trisatech.MWorkforce.Api.Interfaces;
using Trisatech.MWorkforce.Api.ViewModels;
using Trisatech.MWorkforce.Domain;
using Trisatech.MWorkforce.Domain.Entities;
using Trisatech.AspNet.Common.Interfaces;
using Trisatech.AspNet.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Api.Services
{
    public class RefService:IRefService
    {
        private IUnitOfWork unitOfWork;
        private IRepository<RefAssigment> repoRef;

        public RefService(MobileForceContext context)
        {
            unitOfWork = new UnitOfWork<MobileForceContext>(context);
            repoRef = this.unitOfWork.GetRepository<RefAssigment>();
        }

        public IEnumerable<ReasonViewModel> GetReasons()
        {
            try
            {
                repoRef.Condition = PredicateBuilder.True<RefAssigment>().And(x => x.IsDeleted == 0);
                var result = repoRef.Find<ReasonViewModel>(x => new ReasonViewModel
                {
                    Code = x.Code,
                    Name = x.Name
                }).ToList();

                return result;
            }catch(ApplicationException appEx)
            {
                throw appEx;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
