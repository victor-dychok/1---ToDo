using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Application.Abstraction.Percistance
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        Task<TEntity[]> GetListAsync(
            int? offset = null,
            int? limit = null,
            Expression<Func<TEntity, bool>>? predicate = null,
            Expression<Func<TEntity, object>>? orderBy = null,
            bool? destinct = null,
            CancellationToken token = default);
        Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);
        Task<TEntity?> AddAsync(TEntity item, CancellationToken cancellationToken = default);
        Task<TEntity?> UpdateAsync(TEntity item, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(TEntity item, CancellationToken token = default);
        Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken token = default);
    }
}
