using ExaminationSystem.Common;
using ExaminationSystem.Models;
using MediatR;

namespace ExaminationSystem.Features.Quizzes.CreateQuiz
{
    public record CreateQuizCommand(
        string Title,
        Guid DiplomaId,
        int DurationMinutes,
        decimal? PassScore,
        int? MaxAttempts,
        string? Instructions
        ) : IRequest<Result<CreateQuizResponse>>;
}
