using FluentValidation;
using MediatR;
using ValidationException = FluentValidation.ValidationException;

namespace Common.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponce> : IPipelineBehavior<TRequest, TResponce>
    {
        private readonly IValidator<TRequest> _validator;

        public ValidationBehavior(IValidator<TRequest> validator)
        {
            _validator = validator;
        }

        public async Task<TResponce> Handle(TRequest request, RequestHandlerDelegate<TResponce> next, CancellationToken cancellationToken)
        {
            var result = await _validator.ValidateAsync(request, cancellationToken);
            if(!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            return await next();
        }
    }
}
