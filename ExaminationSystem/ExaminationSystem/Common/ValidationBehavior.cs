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
            RequestHandlerDelegate<TResponse> next,  // ← ده الـ Handler اللي بعدنا
            CancellationToken cancellationToken)
        {
            // 1️⃣ لو مفيش Validators للـ Request ده → كمّل عادي
            if (!_validators.Any())
                return await next();
            // 2️⃣ شغّل كل الـ Validators على الـ Request
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            // 3️⃣ اجمع كل الأخطاء من كل الـ Validators
            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();
            // 4️⃣ لو فيه أخطاء → ارمي Exception (مش هيوصل للـ Handler)
            if (failures.Any())
                throw new ValidationException(failures);
            // 5️⃣ لو مفيش أخطاء → كمّل للـ Handler عادي
            return await next();
        }


    }
}
