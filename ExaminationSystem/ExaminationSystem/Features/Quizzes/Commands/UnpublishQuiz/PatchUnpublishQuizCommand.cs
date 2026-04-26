using ExaminationSystem.Common;
using MediatR;

namespace ExaminationSystem.Features.PublishQuiz
{

    public record PatchUnpublishQuizCommand(Guid QuizId): IRequest<Result<string>>;
}
