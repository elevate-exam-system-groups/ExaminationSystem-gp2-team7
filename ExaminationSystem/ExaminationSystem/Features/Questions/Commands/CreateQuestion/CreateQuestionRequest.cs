namespace ExaminationSystem.Features.Questions.Commands.CreateQuestion
{
    /// <summary>
    /// شكل الـ JSON اللي الـ Admin بيبعته (بدون QuizId — ده من الـ URL)
    /// </summary>
    public class CreateQuestionRequest
    {
        public string Text { get; set; } = default!;
        public List<OptionDto> Options { get; set; } = [];
        public string? Explanation { get; set; }
    }
}
