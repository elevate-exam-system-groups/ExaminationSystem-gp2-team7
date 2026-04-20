using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;

namespace ExaminationSystem.Features.Diplomas.Queries.GetDiplomaQuizzes
{
    public class DiplomaQuizResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public int DurationMinutes { get; set; }
        public int AttemptCount { get; set; }
        public decimal? LastScore { get; set; }
        public AttemptStatus? Status { get; set; }
    }
}
