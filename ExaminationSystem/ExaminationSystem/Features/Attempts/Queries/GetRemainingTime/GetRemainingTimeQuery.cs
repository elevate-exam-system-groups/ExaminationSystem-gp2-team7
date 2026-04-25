using ExaminationSystem.Common;
using ExaminationSystem.Contracts;
using MediatR;

namespace ExaminationSystem.Features.Attempts.Queries.GetRemainingTime
{
    public record GetRemainingTimeQuery(Guid AttemptId, Guid UserId, bool IsAdmin) 
        : IRequest<Result<RemainingTimeResponse>>, IActiveAttemptRequest;
}
