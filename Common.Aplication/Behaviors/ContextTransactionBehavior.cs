using Common.Application.Abstraction.Percistance;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Application.Behaviors
{
    public class ContextTransactionBehavior<TRequest, TRespounce> : IPipelineBehavior<TRequest, TRespounce>
    {
        private readonly IContextTransactionCreater _contextTransactionCreater;

        public ContextTransactionBehavior(IContextTransactionCreater contextTransactionCreater) 
        {
            _contextTransactionCreater = contextTransactionCreater;
        }
        public async Task<TRespounce> Handle(TRequest request, RequestHandlerDelegate<TRespounce> next, CancellationToken cancellationToken)
        {
            await using var transaction = await _contextTransactionCreater.BeginTransaction(cancellationToken);
            {
                try
                {
                    var result = await next();
                    await transaction.CommitAsync(cancellationToken);
                    return result;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(CancellationToken.None);
                    throw;
                }
            }
        }
    }
}
