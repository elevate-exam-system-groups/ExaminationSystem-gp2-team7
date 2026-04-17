using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;

namespace ExaminationSystem.Features.Quizzes.Commands.CreateQuiz
{
    public class CreateQuizResponse
    {
        public Guid QuizId { get; set; }
        public string Title { get; set; }
        public Guid DiplomaId { get; set; }
        public int DurationMinutes { get; set; }
        public decimal PassScore { get; set; }
        public int? MaxAttempts { get; set; }
        public string? Instructions { get; set; }
        public QuizStatus Status { get; set; }
    }
}
