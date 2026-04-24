using ExaminationSystem.Models.Enums;

namespace ExaminationSystem.Features.Attempts.Queries.GetAttemptResults.Helpers
{
    public sealed class AttemptMeta
    {
        public Guid StudentId { get; init; }
        public AttemptStatus Status { get; init; }
    }

    public sealed class AttemptProjection
    {
        public Guid Id { get; init; }
        public decimal? Score { get; init; }
        public int? TotalQuestions { get; init; }
        public int? CorrectAnswers { get; init; }
        public bool? Passed { get; init; }
        public DateTime? SubmittedAt { get; init; }
        public string QuizTitle { get; init; } = default!;
        public decimal PassScore { get; init; }
        public List<AnswerProjection> Answers { get; init; } = [];
    }

    public sealed class AnswerProjection
    {
        public Guid QuestionId { get; init; }
        public string QuestionText { get; init; } = default!;
        public string QuestionType { get; init; } = default!;
        public bool IsCorrect { get; init; }
        public Guid? SelectedOptionId { get; init; }
        public string? SelectedOptionText { get; init; }
        public bool? StudentAnswer { get; init; }
    }

    public sealed class CorrectOptionProjection
    {
        public Guid QuestionId { get; init; }
        public Guid OptionId { get; init; }
        public string OptionText { get; init; } = default!;
        public string? Explanation { get; init; }
    }
}
