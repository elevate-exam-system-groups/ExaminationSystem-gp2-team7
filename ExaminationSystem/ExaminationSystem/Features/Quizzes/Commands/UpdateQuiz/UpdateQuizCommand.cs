using ExaminationSystem.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace ExaminationSystem.Features.Quizzes.Commands.UpdateQuiz
{
    // Positional Records
    public record UpdateQuizCommand(
        Guid AdminId,
        Guid QuizId,
        string Title,
        int DurationMinutes,
        decimal PassScore,
        int? MaxAttempts,
       string Instructions
        ) : IRequest<Result<UpdateQuizResponse>>;
}
