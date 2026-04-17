namespace ExaminationSystem.Features.Quizzes.Commands.CreateQuiz
{
    public record CreateQuizRequest(
        string Title,
        Guid DiplomaId,
        int DurationMinutes,
        decimal PassScore,
        int? MaxAttempts,
        string? Instructions
        );
}
