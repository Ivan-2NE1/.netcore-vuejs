using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace VSAND.Data.Repositories
{
    public interface IRepository<T> where T: class
    {
        Task Insert(T entity);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entity);
        Task Delete(int id);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        Task<int> Count();
        Task<int> Count(Expression<Func<T, bool>> filter);
        Task<T> GetById(int id);
        Task<T> Single();
        Task<T> Single(Expression<Func<T, bool>> filter);
        Task<T> Single(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);
        Task<T> Single(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, List<string> includeProperties);
        Task<List<T>> List();
        Task<List<T>> List(Expression<Func<T, bool>> filter);
        Task<List<T>> List(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);
        Task<List<T>> List(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, List<string> includeProperties);
        Task<PagedResult<T>> PagedList(Expression<Func<T, bool>> filter,  Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, List<string> includeProperties, int pageNum, int pageSize);
        Task<bool> UpdateOrder(IEnumerable<T> newList, Func<T, bool> dbListFilter, Func<T, int> getIdFunc, Action<T, int> setSortOrderAction);
    }
}
