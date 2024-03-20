using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Repository
{
    public class SqlServerBaseReository<TEntity>(AppDBContext applicationDbContext) : IRepository<TEntity> where TEntity : class, new()
    {
        private readonly AppDBContext _applicationDbContext = applicationDbContext;

        public async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            var set = _applicationDbContext.Set<TEntity>();
            return predicate == null ? await set.SingleOrDefaultAsync(cancellationToken) : await set.SingleOrDefaultAsync(predicate, cancellationToken);
        }

        public int Count(Expression<Func<TEntity, bool>>? predicate = null)
        {
            var set = _applicationDbContext.Set<TEntity>();
            return predicate == null ? set.Count() : set.Count(predicate);
        }

        public async Task<TEntity?> AddAsync(TEntity item, CancellationToken cancellationToken = default)
        {
            await _applicationDbContext.AddAsync(item, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return item;
        }

        public async Task<TEntity[]> GetListAsync(int? offset = null, int? limit = null, Expression<Func<TEntity, bool>>? predicate = null, Expression<Func<TEntity, object>>? orderBy = null, bool? destinct = null, CancellationToken token = default)
        {
            var queryable = _applicationDbContext.Set<TEntity>().AsQueryable();

            if (predicate is not null)
            {
                queryable = queryable.Where(predicate);
            }

            if (orderBy is not null)
            {
                queryable = destinct == true ? queryable.OrderByDescending(orderBy) : queryable.OrderBy(orderBy);
            }

            if (offset.HasValue)
            {
                queryable = queryable.Skip(offset.Value);
            }

            if (limit.HasValue)
            {
                queryable = queryable.Take(limit.Value);
            }
            return queryable.ToArray();
        }

        public async Task<TEntity?> UpdateAsync(TEntity item, CancellationToken cancellationToken = default)
        {
            _applicationDbContext.Update(item);
            await _applicationDbContext.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteAsync(TEntity item, CancellationToken token = default)
        {
            var set = _applicationDbContext.Set<TEntity>();
            set.Remove(item);
            return await _applicationDbContext.SaveChangesAsync() > 0;
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            var set = _applicationDbContext.Set<TEntity>();
            return predicate == null ? await set.FirstOrDefaultAsync(cancellationToken) : await set.FirstOrDefaultAsync(predicate, cancellationToken);

        }
    }
}