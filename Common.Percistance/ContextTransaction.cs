using Common.Application.Abstraction.Percistance;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Percistance
{
    public class ContextTransaction : IContextTransaction
    {
        private readonly IDbContextTransaction _contextTransaction;

        public ContextTransaction(IDbContextTransaction contextTransaction) 
        {
            _contextTransaction = contextTransaction;
        }
        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await _contextTransaction.CommitAsync(cancellationToken);
        }

        public void Dispose()
        {
            _contextTransaction.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            return _contextTransaction.DisposeAsync();
        }

        public async Task RollbackAsync(CancellationToken cancellationToken)
        {
            await _contextTransaction.RollbackAsync(cancellationToken);
        }
    }
}
