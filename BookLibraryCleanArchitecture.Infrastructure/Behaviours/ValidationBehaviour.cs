using FluentValidation;
using MediatR;

namespace BookLibraryCleanArchitecture.Infrastructure.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, 
            RequestHandlerDelegate<TResponse> next, 
            CancellationToken cancellationToken)
        {
            if(_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = _validators
                    .Select(v => v.Validate(context))
                    .SelectMany(result => result.Errors)
                    .Where(failure => failure != null)
                    .ToList();

                if (validationResults.Any())
                {
                    throw new ValidationException(validationResults);
                }
            }

            return await next();
        }
    }
}
