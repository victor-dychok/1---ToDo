using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Infrastructure.Common.Percistance
{
    public class BaseRepository<TEntity>// : IRepository<TEntity> where TEntity : class, new()
    {
        private readonly List<TEntity> _data = new List<TEntity>();

        public TEntity[] GetList(
            int? offset = null,
            int? limit = null,
            Expression<Func<TEntity, bool>>? predicate = null,
            Expression<Func<TEntity, object>>? orderBy = null,
            bool? desending = null)
        {

            IEnumerable<TEntity> result = _data;

            if(predicate !=  null)
            {
                result = result.Where(predicate.Compile());
            }


            if (orderBy != null)
            {
                result = desending.GetValueOrDefault() ? result.OrderByDescending(orderBy.Compile()) : result.OrderBy(orderBy.Compile());
            }

            result = result.Skip(offset.GetValueOrDefault());
            
            if(limit.HasValue)
            {
                result = result.Take(limit.Value);
            }
            var res = result.ToList();

            return result.ToArray();
        }
        public TEntity? Add(TEntity item)
        {
            _data.Add(item);
            return item;
        }

        public bool Delete(TEntity item)
        {
            return _data.Remove(item);
        }

        public TEntity? Update(TEntity item)
        {
            _data.Remove(item);
            _data.Add(item);
            return item;
        }

        public TEntity? SingleOrDefault(Expression<Func<TEntity, bool>>? predicate = null)
        {
            if(predicate is null)
            {
                return _data.SingleOrDefault();
            }
            return _data.SingleOrDefault(predicate.Compile());
        }

        public Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity?> AddAsync(TEntity item, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity[]> GetListAsync(int? offset = null, int? limit = null, Expression<Func<TEntity, bool>>? predicate = null, Expression<Func<TEntity, object>>? orderBy = null, bool? destinct = null)
        {
            throw new NotImplementedException();
        }
    }
}
