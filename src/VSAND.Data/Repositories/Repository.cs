using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace VSAND.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        internal VsandContext Context;
        internal DbSet<T> DbSet;

        public Repository(VsandContext context)
        {
            Context = context ?? throw new ArgumentException("context");
            DbSet = context.Set<T>();
        }

        public virtual async Task Insert(T entity)
        {
            await DbSet.AddAsync(entity);
        }

        public virtual void Update(T entity)
        {
            Context.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            Context.UpdateRange(entities);
        }

        public virtual async Task Delete(int id)
        {
            T existing = await GetById(id);
            Delete(existing);
        }

        public virtual void Delete(T entity)
        {
            if (entity != null)
            {
                if (Context.Entry(entity).State == EntityState.Detached)
                {
                    DbSet.Attach(entity);
                }

                DbSet.Remove(entity);
            }
        }

        public virtual void DeleteRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                if (entity != null)
                {
                    if (Context.Entry(entity).State == EntityState.Detached)
                    {
                        DbSet.Attach(entity);
                    }
                }
            }

            DbSet.RemoveRange(entities);
        }

        public async Task<int> Count()
        {
            return await DbSet.CountAsync();
        }

        public async Task<int> Count(Expression<Func<T, bool>> filter)
        {
            return await DbSet.CountAsync(filter);
        }

        public virtual async Task<T> GetById(int id)
        {
            var oRet = await DbSet.FindAsync(id);
            if (oRet != null)
            {
                Context.Entry(oRet).State = EntityState.Detached;
            }
            return oRet;
        }

        public virtual Task<T> Single()
        {
            return Single(null, null, null);
        }

        public async Task<T> Single(Expression<Func<T, bool>> filter)
        {
            return await Single(filter, null, null);
        }

        public async Task<T> Single(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            return await Single(filter, orderBy, null);
        }

        public async Task<T> Single(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, List<string> includeProperties)
        {
            var oRet = await GetQueryable(filter, orderBy, includeProperties).FirstOrDefaultAsync();
            if (oRet != null)
            {
                Context.Entry(oRet).State = EntityState.Detached;
            }
            return oRet;
        }

        public async Task<List<T>> List()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<List<T>> List(Expression<Func<T, bool>> filter)
        {
            return await List(filter, null, null);
        }

        public async Task<List<T>> List(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            return await List(filter, orderBy, null);
        }

        public async Task<List<T>> List(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, List<string> includeProperties)
        {
            return await GetQueryable(filter, orderBy, includeProperties).ToListAsync();
        }

        public async Task<PagedResult<T>> PagedList(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, List<string> includeProperties, int pageNum, int pageSize)
        {
            var query = GetQueryable(filter, orderBy, includeProperties);
            int totalResults = query.Count();

            pageNum -= 1;

            if (pageNum < 0)
            {
                pageNum = 0;
            }

            query = query.Skip(pageNum * pageSize).Take(pageSize);

            var oRet = new PagedResult<T>(null, 0, pageSize, pageNum);
            oRet.Results = await query.ToListAsync();
            oRet.TotalResults = totalResults;
            return oRet;
        }

        public async Task<bool> UpdateOrder(IEnumerable<T> newList, Func<T, bool> dbListFilter, Func<T, int> getIdFunc, Action<T, int> setSortOrderAction)
        {
            // to prevent accidentally updating Game Meta information, pull a fresh copy from the DB and only update the order
            var dbList = await List(x => dbListFilter.Invoke(x));

            // make sure that the given list is consistent with the DB
            var dbListIds = dbList.Select(x => getIdFunc.Invoke(x)).ToList();
            var newListIds = newList.Select(x => getIdFunc.Invoke(x)).ToList();

            if (newListIds.Count != dbListIds.Count || !newListIds.All(dbListIds.Contains))
            {
                return false;
            }

            // iterate and assign new sort order
            int i = 1;
            foreach (var entity in newList)
            {
                setSortOrderAction.Invoke(dbList.FirstOrDefault(x => getIdFunc.Invoke(x) == getIdFunc.Invoke(entity)), i);
                i++;
            }

            UpdateRange(dbList);

            return true;
        }

        private IQueryable<T> GetQueryable(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, List<string> includeProperties)
        {
            IQueryable<T> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    var includeProp = includeProperty.Trim();
                    if (includeProp != "")
                    {
                        query = query.Include(includeProp);
                    }
                }
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }

            return query;
        }
    }
}



