using ExaminationSystem.Models;

namespace ExaminationSystem.Features.Quizzes.Commands.StartQuiz
{
    public class StartQuizResponse
    {
        public Guid AttemptId { get; set; }
        public QuizInfoDto quiz { get; set; }
        public List<QuestionDto> questions { get; set; }
        public DateTime StartTime { get; set; }
    }

    public class QuizInfoDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int DurationMinutes { get; set; }
        public decimal PassScore { get; set; }
        public string Instructions { get; set; }
    }

    public class QuestionDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }       // "MCQ" or "TrueFalse"
        public int OrderIndex { get; set; }
        public List<OptionDto>? Options { get; set; }
    }
    public class OptionDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public int OrderIndex { get; set; }
        // No IsCorrect — must NOT be revealed
    }


}
