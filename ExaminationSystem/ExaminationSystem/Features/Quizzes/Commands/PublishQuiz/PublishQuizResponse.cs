namespace ExaminationSystem.Features.Quizzes.Commands.PublishQuiz
{
    public class PublishQuizResponse
    {
        public Guid QuizId { get; set; }
        public string Status { get; set; } = default!;
    }
}

