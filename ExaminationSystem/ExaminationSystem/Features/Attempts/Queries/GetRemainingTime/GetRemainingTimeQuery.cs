using ExaminationSystem.Common;
using ExaminationSystem.Contracts;
using MediatR;

namespace ExaminationSystem.Features.Attempts.Queries.GetRemainingTime
{
    public record GetRemainingTimeQuery(Guid AttemptId, Guid UserId, bool IsAdmin)
        : IActiveAttemptRequest<Result<RemainingTimeResponse>>
    {
        public Result<RemainingTimeResponse> CreateTimedOutResponse()
        => Result<RemainingTimeResponse>.Failure(
                "Timer expired. Attempt has been auto-submitted.",
                StatusCodes.Status410Gone);
    }
}
