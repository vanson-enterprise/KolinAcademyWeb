using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KA.Service.Base
{
    public interface IService<T> where T : class
    {
        #region Nomarl function

        T GetById(object id);

        IQueryable<T> GetAll();

        T Find(Expression<Func<T, bool>> match);

        ICollection<T> FindAll(Expression<Func<T, bool>> match);

        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);

        T Add(T entity);

        int AddMany(IEnumerable<T> entities);

        int Update(T entity);

        int Delete(T entity);

        int DeleteById(object id);

        #endregion

        #region Async function

        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match);

        Task<T> FindAsync(Expression<Func<T, bool>> match);

        Task<T> GetByIdAsync(object id);

        Task<IList<T>> GetAllAsync();

        Task<T> AddAsync(T entity);

        Task<int> AddManyAsync(IEnumerable<T> entities);

        Task<int> UpdateAsync(T entity);

        Task<int> DeleteAsync(T entity);

        #endregion
    }
}
