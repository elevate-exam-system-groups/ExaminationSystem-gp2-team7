using ExaminationSystem.Common;
using ExaminationSystem.Features.Attempts.Queries.GetAttemptResults;
using MediatR;

namespace ExaminationSystem.Features.Attempts.Queries.ViewResults
{
    public record GetAttemptResultsQuery(
        Guid AttemptId,
        Guid UserId,
        bool IsAdmin) : IRequest<Result<AttemptResultsResponse>>;
}
