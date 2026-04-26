namespace ExaminationSystem.Features.Attempts.Queries.GetStudentAttempts
{
    public class AttemptDto
    {
        public Guid AttemptId { get; set; }
        public Guid StudentId { get; set; }
        public string? StudentName { get; set; }
        public string QuizTitle { get; set; } = default!;
        public decimal? Score { get; set; }
        public string Status { get; set; } = default!;
        public DateTime SubmittedAt { get; set; }
    }
}
