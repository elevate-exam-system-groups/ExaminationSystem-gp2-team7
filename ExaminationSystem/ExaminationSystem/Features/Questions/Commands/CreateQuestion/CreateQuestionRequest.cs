namespace ExaminationSystem.Features.Questions.Commands.CreateQuestion
{
    
    public class CreateQuestionRequest
    {
        public string Text { get; set; } = default!;
        public string QuestionType { get; set; } = default!;

       
        public List<OptionDto>? Options { get; set; }
        public string? Explanation { get; set; }

       
        public bool? CorrectAnswer { get; set; }
    }
}
