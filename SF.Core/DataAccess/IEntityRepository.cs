using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SF.Core.Entities;

namespace SF.Core.DataAccess
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        T Get(Expression<Func<T, bool>> filter);
        int Count(Expression<Func<T, bool>> filter);
        IList<T> GetList(Expression<Func<T, bool>> filter = null);
        T Add(T entity);
        T Update(T entity);
        void Delete(T entity);
        int DeleteRange(Expression<Func<T, bool>> filter);


    }
}
