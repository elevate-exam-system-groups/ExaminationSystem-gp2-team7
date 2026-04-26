using ExaminationSystem.Common;
using MediatR;

namespace ExaminationSystem.Features.Questions.Commands.DeleteQuestion
{
    
    public record DeleteQuestionCommand(Guid QuestionId) : IRequest<Result<bool>>;
}
