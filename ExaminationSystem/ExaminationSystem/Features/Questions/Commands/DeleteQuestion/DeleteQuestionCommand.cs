using ExaminationSystem.Common;
using MediatR;

namespace ExaminationSystem.Features.Questions.Commands.DeleteQuestion
{
    /// <summary>
    /// أمر حذف سؤال — محتاج الـ QuestionId بس
    /// </summary>
    public record DeleteQuestionCommand(Guid QuestionId) : IRequest<Result<bool>>;
}
