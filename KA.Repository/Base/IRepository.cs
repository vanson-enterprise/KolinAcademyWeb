using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace KA.Repository.Base
{
    public interface IRepository<T> where T : class
    {
        #region normal function

        T GetById(object id);

        IQueryable<T> GetAll();
        IQueryable<T> GetAllNoTracking();

        T Find(Expression<Func<T, bool>> match);

        ICollection<T> FindAll(Expression<Func<T, bool>> match);

        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);

        T Add(T entity);

        int AddMany(IEnumerable<T> entities);

        int Update(T entity);


        int Delete(T entity);

        int DeleteById(object id);

        #endregion

        #region Async Reponsitory

        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match);

        Task<T> FindAsync(Expression<Func<T, bool>> match);

        Task<T> GetByIdAsync(object id);

        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

        Task<IList<T>> GetAllAsync();

        Task<T> AddAsync(T entity);

        Task<int> AddManyAsync(IEnumerable<T> entities);

        Task<int> UpdateAsync(T entity);

        Task<int> UpdateRangeAsync(List<T> entities);

        Task<int> DeleteAsync(T entity);

        #endregion
    }
}
