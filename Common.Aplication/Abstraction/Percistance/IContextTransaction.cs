using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Application.Abstraction.Percistance
{
    public interface IContextTransaction : IDisposable, IAsyncDisposable
    {

        Task CommitAsync(CancellationToken cancellationToken);
        Task RollbackAsync(CancellationToken cancellationToken);
    }
}
