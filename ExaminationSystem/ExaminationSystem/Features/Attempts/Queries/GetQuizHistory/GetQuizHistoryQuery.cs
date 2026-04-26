using ExaminationSystem.Common;
using ExaminationSystem.Features.Diplomas.Queries.GetAllDiplomas;
using MediatR;

namespace ExaminationSystem.Features.Attempts.Queries.GetQuizHistory
{
    public record GetQuizHistoryQuery : IRequest<Result<PagedResponse<QuizHistoryItemResponse>>>
    {
        public Guid StudentId { get; init; }
        public Guid? QuizId { get; init; }
        public Guid? DiplomaId { get; init; }
        public int Page { get; init; } = 1;
        public int PerPage { get; init; } = 10;
    }
}
