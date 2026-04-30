using FluentValidation;
using MediatR;

namespace ExaminationSystem.Common
{
    public class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
           
            if (!_validators.Any())
                return await next();

          
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

          
            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

           
            if (failures.Any())
            {
                var errorMessage = string.Join(" | ", failures.Select(f => f.ErrorMessage));

               
                var responseType = typeof(TResponse);
                if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
                {
                    var failureMethod = responseType.GetMethod("Failure",
                        new[] { typeof(string), typeof(int) });

                    if (failureMethod != null)
                    {
                        var result = failureMethod.Invoke(null,
                            new object[] { errorMessage, StatusCodes.Status422UnprocessableEntity });
                        return (TResponse)result!;
                    }
                }

                
                throw new ValidationException(failures);
            }

          
            return await next();
        }
    }
}
