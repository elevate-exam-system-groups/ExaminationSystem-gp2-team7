using ExaminationSystem.Models;

namespace ExaminationSystem.Features.Diplomas.Queries.GetAllDiplomas
{
    public class DiplomaResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public decimal Progress { get; set; } = 0m;
        public int TotalQuizCount { get; set; }
    }
}