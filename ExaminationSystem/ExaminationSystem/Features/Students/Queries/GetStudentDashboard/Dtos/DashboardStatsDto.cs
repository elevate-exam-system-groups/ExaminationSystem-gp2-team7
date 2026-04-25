namespace ExaminationSystem.Features.Students.Queries.GetStudentDashboard.Dtos
{
    public class DashboardStatsDto
    {
        public int TotalQuizzesTaken { get; set; }
        public decimal AvgScore { get; set; }
        public double PassRate { get; set; }
    }
}
