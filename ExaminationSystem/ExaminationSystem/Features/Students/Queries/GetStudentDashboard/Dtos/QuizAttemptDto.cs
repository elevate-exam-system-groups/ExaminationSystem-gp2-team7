namespace ExaminationSystem.Features.Students.Queries.GetStudentDashboard.Dtos
{
    public class QuizAttemptDto
    {
        public Guid QuizId { get; set; }
        public decimal? Score { get; set; }
        public DateTime? Date { get; set; }
    }
}
