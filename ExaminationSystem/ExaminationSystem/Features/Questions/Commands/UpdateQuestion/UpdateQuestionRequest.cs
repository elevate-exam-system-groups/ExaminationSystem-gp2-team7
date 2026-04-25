namespace ExaminationSystem.Features.Questions.Commands.UpdateQuestion
{
    /// <summary>
    /// شكل الـ JSON body (بدون QuestionId — ده من الـ URL)
    /// </summary>
    public class UpdateQuestionRequest
    {
        public string Text { get; set; } = default!;
        public List<CreateQuestion.OptionDto> Options { get; set; } = [];
        public string? Explanation { get; set; }
    }
}
