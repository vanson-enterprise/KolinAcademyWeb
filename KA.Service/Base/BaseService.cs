using KA.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KA.Service.Base
{
    public class BaseService<T> : IService<T> where T : class
    {
        private readonly IRepository<T> _baseReponsitory;
        public BaseService(IRepository<T> baseReponsitory)
        {
            _baseReponsitory = baseReponsitory;
        }

        #region Async
        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match)
        {
            return await _baseReponsitory.FindAllAsync(match);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await _baseReponsitory.FindAsync(match);
        }

        public virtual async Task<T> GetByIdAsync(object id)
        {
            return await _baseReponsitory.GetByIdAsync(id);
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await _baseReponsitory.GetAllAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            return await _baseReponsitory.AddAsync(entity);
        }

        public async Task<int> AddManyAsync(IEnumerable<T> entities)
        {
            return await _baseReponsitory.AddManyAsync(entities);
        }

        public async Task<int> UpdateAsync(T entity)
        {
            return await _baseReponsitory.UpdateAsync(entity);
        }

        public async Task<int> DeleteAsync(T entity)
        {
            return await _baseReponsitory.DeleteAsync(entity);
        }

        #endregion

        #region Normal

        public T GetById(object id)
        {
            return _baseReponsitory.GetById(id);
        }

        public IQueryable<T> GetAll()
        {
            return _baseReponsitory.GetAll();
        }

        public T Find(Expression<Func<T, bool>> match)
        {
            return _baseReponsitory.Find(match);
        }

        public ICollection<T> FindAll(Expression<Func<T, bool>> match)
        {
            return _baseReponsitory.FindAll(match);
        }

        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            return _baseReponsitory.GetAllIncluding(includeProperties);
        }

        public virtual T Add(T entity)
        {
            return _baseReponsitory.Add(entity);
        }

        public int AddMany(IEnumerable<T> entities)
        {
            return _baseReponsitory.AddMany(entities);
        }

        public int Update(T entity)
        {
            return _baseReponsitory.Update(entity);
        }

        public int Delete(T entity)
        {
            return _baseReponsitory.Delete(entity);
        }

        public virtual int DeleteById(object id)
        {
            return _baseReponsitory.DeleteById(id);
        }
        #endregion
    }
}
