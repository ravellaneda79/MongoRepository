// <copyright company="Roche Diagnostics AG">
// Copyright (c) Roche Diagnostics AG. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MongoRepositoryDAL
{
    public interface IGenericMongoRepository<TEntity> where TEntity : class
    {
        void Save(TEntity entity);
        void Save(IEnumerable<TEntity> entities);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, int limit);
        void Update<TUEntity>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TUEntity>> updateField, TUEntity newValue) where TUEntity : class;
        void Delete(Expression<Func<TEntity, bool>> predicate);
        void DropCollection();
    }
}