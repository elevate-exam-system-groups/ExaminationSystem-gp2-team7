namespace ExaminationSystem.Features.Students.Queries.GetStudentDashboard.Dtos
{
    public class StudentDashboardDto
    {
        public List<EnrolledDiplomaDto> EnrolledDiplomas { get; set; } = [];
        public List<QuizAttemptDto> RecentQuizAttempts { get; set; } = [];
        public DashboardStatsDto OverallStats { get; set; } = new();
    }
}
