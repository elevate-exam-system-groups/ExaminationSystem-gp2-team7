using ExaminationSystem.Common;
using ExaminationSystem.Features.Questions.Commands.CreateQuestion;
using MediatR;

namespace ExaminationSystem.Features.Questions.Commands.UpdateQuestion
{
    public record UpdateQuestionCommand : IRequest<Result<bool>>
    {
        public Guid QuestionId { get; init; }
        public string Text { get; init; } = default!;

        // MCQ
        public List<OptionDto>? Options { get; init; }
        public string? Explanation { get; init; }

        // TrueFalse
        public bool? CorrectAnswer { get; init; }
    }
}
