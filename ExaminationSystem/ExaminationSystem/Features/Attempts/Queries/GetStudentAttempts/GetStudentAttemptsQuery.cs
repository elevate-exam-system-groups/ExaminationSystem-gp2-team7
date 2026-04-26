using ExaminationSystem.Common;
using MediatR;

namespace ExaminationSystem.Features.Attempts.Queries.GetStudentAttempts
{
    public record GetStudentAttemptsQuery(
                                        Guid? QuizId,
                                        Guid? StudentId,
                                        string? SortBy = "submitted_at",
                                        string? Order = "desc",
                                        int Page = 1,
                                        int PerPage = 20
                                       ) 
        : IRequest<Result<PaginatedResult<AttemptDto>>>;
}
