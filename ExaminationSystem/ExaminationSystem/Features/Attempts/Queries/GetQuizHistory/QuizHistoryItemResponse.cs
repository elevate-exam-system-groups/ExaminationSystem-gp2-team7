namespace ExaminationSystem.Features.Attempts.Queries.GetQuizHistory
{
    public record QuizHistoryItemResponse
    {
        public Guid AttemptId { get; init; }
        public string QuizTitle { get; init; } = default!;
        public decimal? Score { get; init; }
        public bool? Passed { get; init; }
        public string Status { get; init; } = default!;
        public DateTime? SubmittedAt { get; init; }
    }
}
