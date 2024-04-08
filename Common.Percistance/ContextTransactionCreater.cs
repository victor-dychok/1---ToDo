using Application.Infrastructure.Common.Percistance;
using Common.Application.Abstraction.Percistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Percistance
{
    public class ContextTransactionCreater : IContextTransactionCreater
    {
        private readonly AppDBContext _dbContext;
        public ContextTransactionCreater(AppDBContext context) 
        {
            _dbContext = context;
        }
        public async Task<IContextTransaction> BeginTransaction(CancellationToken cancellationToken)
        {
            return new ContextTransaction(await _dbContext.Database.BeginTransactionAsync(cancellationToken));
        }
    }
}
