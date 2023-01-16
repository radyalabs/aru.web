using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Trisatech.AspNet.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trisatech.AspNet.Common.Repository
{
    public class UnitOfWork<C> : IUnitOfWork where C : DbContext
    {
        private IDbContextTransaction _transaction;
        private IExecutionStrategy _strategy;
        private Dictionary<Type, object> _repositories;

        private C _ctx;

        public UnitOfWork(DbContext dbContext)
        {
            _ctx = (C)dbContext;
            _repositories = new Dictionary<Type, object>();
        }

        public IExecutionStrategy Strategy()
        {
            _strategy = _ctx.Database.CreateExecutionStrategy();

            return _strategy;
        }

        public IRepository<TSet> GetRepository<TSet>() where TSet : class
        {
            if(_repositories == null)
            {
                throw new ArgumentNullException("Repositories cannot be null");
            }

            if (_repositories.Keys.Contains(typeof(TSet)))
            {
                return _repositories[typeof(TSet)] as IRepository<TSet>;
            }
            var repository = new SqlRepository<TSet, C>(_ctx);

            _repositories.Add(typeof(TSet), repository);

            return repository;
        }

        public void BeginTransaction()
        {
            if (_transaction == null)
            {
                _transaction = _ctx.Database.BeginTransaction();
            }
        }

        public void Commit()
        {
            if (_ctx.SaveChanges() >= 0)
            {
                try
                {
                    if (_transaction != null)
                        _transaction.Commit();
                }
                catch
                {
                    if (_transaction != null)
                        _transaction.Rollback();
                }
                finally
                {
                    _transaction.Dispose();
                }
            }
            else
            {
                _transaction.Dispose();
            }
        }

        public void Rollback()
        {
            if (_transaction == null) return;

            try
            {
                _transaction.Rollback();
            }
            finally
            {
                _transaction.Dispose();
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (null != _transaction)
            {
                _transaction.Dispose();
            }

            if (null != _ctx)
            {
                _ctx.Dispose();
            }
        }

        #endregion
    }
}
