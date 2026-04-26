using ExaminationSystem.Common;
using MediatR;

namespace ExaminationSystem.Features.Questions.Commands.CreateQuestion
{
    public record CreateQuestionCommand : IRequest<Result<CreateQuestionResponse>>
    {
        public Guid QuizId { get; init; }
        public string Text { get; init; } = default!;
        public string QuestionType { get; init; } = default!; 

       
        public List<OptionDto>? Options { get; init; }
        public string? Explanation { get; init; }

        public bool? CorrectAnswer { get; init; }
    }

    public class OptionDto
    {
        public string Text { get; set; } = default!;
        public bool IsCorrect { get; set; }
    }
}
