using ExaminationSystem.Common;
using MediatR;

namespace ExaminationSystem.Features.Attempts.Queries.GetAttemptDetail
{
    public record GetAttemptDetailQuery : IRequest<Result<AttemptDetailResponse>>
    {
        public Guid AttemptId { get; init; }
        public Guid StudentId { get; init; }
    }
}
