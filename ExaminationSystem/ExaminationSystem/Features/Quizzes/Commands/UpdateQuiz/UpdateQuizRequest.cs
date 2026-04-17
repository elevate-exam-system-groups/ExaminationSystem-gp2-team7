using ExaminationSystem.Models.Enums;

namespace ExaminationSystem.Features.Quizzes.Commands.UpdateQuiz
{
    public record UpdateQuizRequest(
        string Title,
        int DurationMinutes,
        decimal PassScore,
        int? MaxAttempts,
        string? Instructions
        );
}
