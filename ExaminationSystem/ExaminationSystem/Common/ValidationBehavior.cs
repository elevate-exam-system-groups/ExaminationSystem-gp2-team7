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
            // 1️⃣ لو مفيش Validators → كمّل عادي
            if (!_validators.Any())
                return await next();

            // 2️⃣ شغّل كل الـ Validators
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            // 3️⃣ اجمع الأخطاء
            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            // 4️⃣ لو فيه أخطاء → رجّع Result.Failure بدل throw
            if (failures.Any())
            {
                var errorMessage = string.Join(" | ", failures.Select(f => f.ErrorMessage));

                // تحقق إن TResponse هو Result<T>
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

                // Fallback: لو مش Result<T>
                throw new ValidationException(failures);
            }

            // 5️⃣ مفيش أخطاء → كمّل للـ Handler
            return await next();
        }
    }
}
