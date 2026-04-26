using ExaminationSystem.Common;
using ExaminationSystem.Contracts;
using ExaminationSystem.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ExaminationSystem.Models.Enums;

namespace ExaminationSystem.Features.Attempts.Queries.GetRemainingTime
{
    public class GetRemainingTimeHandler(IGenericRepository<Attempt> repository)
        : IRequestHandler<GetRemainingTimeQuery, Result<RemainingTimeResponse>>
    {
        public async Task<Result<RemainingTimeResponse>> Handle(GetRemainingTimeQuery request, CancellationToken cancellationToken)
        {
            // Validate existing
            var attempt = await repository.AsQueryable()
                .Where(a => a.Id == request.AttemptId)
                .Select(a => new
                {
                    a.StudentId,
                    a.Status,
                    a.Deadline
                })
                .FirstOrDefaultAsync(cancellationToken);

            // Validate existing
            if (attempt == null)
            {
                return Result<RemainingTimeResponse>.Failure
                    ("Attempt not found.", StatusCodes.Status404NotFound);
            }

            // Validation
            var validationResult = GetRemainingTimeValidator.Validate
                (request, attempt.StudentId, attempt.Status);
            if (validationResult != null)
            {
                return validationResult;
            }

            // Calculate SecondsRemaining
            var secondsRemaining = (attempt.Deadline - DateTime.UtcNow).TotalSeconds;

            var finalRemaining = Math.Max(0, (int)secondsRemaining);

            return Result<RemainingTimeResponse>.Success(new RemainingTimeResponse(finalRemaining));
        }

       
    }
}
