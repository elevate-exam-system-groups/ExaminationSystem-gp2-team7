using ExaminationSystem.Features.Questions.Commands.CreateQuestion;

namespace ExaminationSystem.Features.Questions.Commands.UpdateQuestion
{
    public class UpdateQuestionRequest
    {
        public string Text { get; set; } = default!;

        // MCQ
        public List<OptionDto>? Options { get; set; }
        public string? Explanation { get; set; }

        // TrueFalse
        public bool? CorrectAnswer { get; set; }
    }
}
