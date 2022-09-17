using KA.DataProvider;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KA.Repository.Base
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext DbContext;
        public BaseRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        #region Async function       

        public virtual async Task<T> GetByIdAsync(object id)
        {
            return await DbContext.Set<T>().FindAsync(id);
        }

        public virtual async Task<IList<T>> GetAllAsync()
        {
            return await DbContext.Set<T>().ToListAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await DbContext.Set<T>().AddAsync(entity);
            await DbContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<int> AddManyAsync(IEnumerable<T> entities)
        {
            await DbContext.Set<T>().AddRangeAsync(entities);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match)
        {
            return await DbContext.Set<T>().Where(match).ToListAsync();
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await DbContext.Set<T>().SingleOrDefaultAsync(match);
        }

        public Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            try
            {
                IQueryable<T> query = DbContext.Set<T>().AsNoTracking();

                if (include != null) query = include(query);

                query = query.Where(predicate);

                if (orderBy != null) query = orderBy(query);

                return query.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<int> UpdateAsync(T entity)
        {
            try
            {
                DbContext.Entry(entity).State = EntityState.Modified;
                return await DbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public virtual async Task<int> DeleteAsync(T entity)
        {
            DbContext.Set<T>().Remove(entity);
            return await DbContext.SaveChangesAsync();
        }

        #endregion

        #region Normal function

        public virtual T GetById(object id)
        {
            return DbContext.Set<T>().Find(id);

        }

        public virtual T Add(T entity)
        {
            DbContext.Set<T>().Add(entity);
            DbContext.SaveChanges();
            return entity;
        }

        public virtual IQueryable<T> GetAll()
        {
            return DbContext.Set<T>();
        }

        public virtual IQueryable<T> GetAllNoTracking()
        {
            return DbContext.Set<T>().AsNoTracking();
        }

        public virtual int Update(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            return DbContext.SaveChanges();
        }
        public virtual int Delete(T entity)
        {
            DbContext.Set<T>().Remove(entity);
            return DbContext.SaveChanges();
        }

        public virtual int DeleteById(object id)
        {
            var entity = DbContext.Set<T>().Find(id);
            if (entity == null)
            {
                return 0;
            }
            DbContext.Set<T>().Remove(entity);
            return DbContext.SaveChanges();
        }

        public virtual int AddMany(IEnumerable<T> entities)
        {
            DbContext.Set<T>().AddRange(entities);
            return DbContext.SaveChanges();
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = DbContext.Set<T>().Where(predicate);
            return query;
        }

        public T Find(Expression<Func<T, bool>> match)
        {
            return DbContext.Set<T>().SingleOrDefault(match);
        }

        public ICollection<T> FindAll(Expression<Func<T, bool>> match)
        {
            return DbContext.Set<T>().Where(match).ToList();
        }

        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> queryable = GetAll();
            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
            {
                queryable = queryable.Include(includeProperty);
            }

            return queryable;
        }

        public async Task<int> UpdateRangeAsync(List<T> entities)
        {
            DbContext.UpdateRange(entities);
            return await DbContext.SaveChangesAsync();
        }
        #endregion
    }
}
