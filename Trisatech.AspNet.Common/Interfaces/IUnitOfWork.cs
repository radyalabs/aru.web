using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Trisatech.AspNet.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<TSet> GetRepository<TSet>() where TSet : class;
        
        void BeginTransaction();

        IExecutionStrategy Strategy();

        void Commit();

        void Rollback();

        void Dispose();
    }
}
