using ExaminationSystem.Common;
using ExaminationSystem.Models;
using MediatR;

namespace ExaminationSystem.Features.Quizzes.Commands.CreateQuiz
{
    public record CreateQuizCommand(
        Guid AdminId,
        string Title,
        Guid DiplomaId,
        int DurationMinutes,
        decimal? PassScore,
        int? MaxAttempts,
        string? Instructions
        ) : IRequest<Result<CreateQuizResponse>>;
}
