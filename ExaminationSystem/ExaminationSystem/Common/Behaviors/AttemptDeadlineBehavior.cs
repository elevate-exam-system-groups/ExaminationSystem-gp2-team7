using ExaminationSystem.Contracts;
using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ExaminationSystem.Common.Behaviors
{
    public sealed class AttemptDeadlineBehavior<TRequest, TResponse>(
        IUnitOfWork unitOfWork,
        IAttemptAutoSubmitService attemptAutoSubmitService)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IActiveAttemptRequest<TResponse>
    {

        public async Task<TResponse> Handle(
            TRequest request, 
            RequestHandlerDelegate<TResponse> next, 
            CancellationToken cancellationToken)
        {
            var attemptRepo = unitOfWork.GetRepository<Attempt>();

            var attempt = await attemptRepo.AsQueryable()
                .Where(a => a.Id == request.AttemptId)
                .Select(a => new
                {
                    a.Status,
                    a.Deadline
                })
                .FirstOrDefaultAsync(cancellationToken);

            // Check if the attempt is InProgress and the deadline has passed
            if (attempt is not null &&
                attempt.Status == AttemptStatus.InProgress &&
                DateTime.UtcNow > attempt.Deadline) 
            {
                // Trigger auto-submit
                await attemptAutoSubmitService.AutoSubmitAsync(request.AttemptId, cancellationToken);

                return request.CreateTimedOutResponse();

            }

            // Timer is fine, proceed to the actual handler

            return await next();
        }
    }
}
