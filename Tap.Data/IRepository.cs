using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Tap.Data
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll();
        T Get(long id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        T SingleOrDefault(Expression<Func<T, bool>> predicate);
        IQueryable<T> Find(Expression<Func<T, bool>> criteria);
        void Delete(Expression<Func<T, bool>> criteria);
        int Count();
        int Count(Expression<Func<T, bool>> criteria);
    }
}
