using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Tap.Data
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationContext context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;
        public Repository(ApplicationContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }
        public DbSet<T> Entities
        {
            get { return entities; }
            set { entities = value; }
        }
        public IQueryable<T> GetAll()
        {
            return entities;
        }
        public T Get(long id)
        {
            return entities.SingleOrDefault(s => s.Id == id);
        }
        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }
        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.SaveChanges();
        }
        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return entities.FirstOrDefault(predicate);
        }

        public T SingleOrDefault(Expression<Func<T, bool>> predicate)
        {
            return entities.SingleOrDefault(predicate);
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> criteria)
        {
            return entities.Where(criteria);
        }

        public Boolean Any(Expression<Func<T, bool>> criteria)
        {
            return entities.Any(criteria);
        }

        public virtual void Delete(Expression<Func<T, bool>> criteria)
        {
            IEnumerable<T> records = Find(criteria);

            foreach (T record in records)
            {
                entities.Remove(record);
            }

            context.SaveChanges();
        }

        public int Count()
        {
            return entities.Count();
        }

        public int Count(Expression<Func<T, bool>> criteria)
        {
            return entities.Count(criteria);
        }
    }
}
