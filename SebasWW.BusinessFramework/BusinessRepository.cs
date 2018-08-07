using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace SebasWW.BusinessFramework
{
    public abstract class BusinessRepository//<TDbContext> 
//        where TDbContext: DbContext
    {
        DbContext _dbContext;

        public BusinessRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected DbSet<TEntity> DbSet<TEntity>() where TEntity:class 
        {
            return _dbContext.Set<TEntity>(); 
        }

        protected IQueryable<TEntity> Get<TEntity>(
            Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, IOrderedQueryable>> orderBy,
            params Expression<Func<TEntity, object>>[] includeProperties
            ) where TEntity : class
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (where != null)
                query = query.Where(where);

            if (includeProperties != null)
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty)); ;

            if (orderBy != null)
                query = query.OrderBy(orderBy);

            return query;
        }

        protected IQueryable<TEntity> Get<TEntity>(
                Expression<Func<TEntity, bool>> where,
                Expression<Func<TEntity, IOrderedQueryable>> orderBy
            ) where TEntity : class
        {
            return Get<TEntity>(where,orderBy, null);
        }

        protected IQueryable<TEntity> Get<TEntity>(
                Expression<Func<TEntity, bool>> where
            ) where TEntity : class
        {
            return Get<TEntity>(where, null, null);
        }

        protected IQueryable<TEntity> Get<TEntity>() where TEntity : class
        {
            return Get<TEntity>(null, null, null);
        }

        public TEntity FindById<TEntity>(int id) where TEntity:class
        {
            return _dbContext.Set<TEntity>().Find(id);
        }


        #region Edit

        //protected void Create(TEntity item)
        //{
        //    _dbSet.Add(item);
        //}
        //protected void Update(TEntity item)
        //{
        //    _dbContext.Entry(item).State = EntityState.Modified;
        //}
        //protected void Remove(TEntity item)
        //{
        //    _dbSet.Remove(item);  
        //}
#endregion
    }
}
