using ExaminationSystem.Common;
using ExaminationSystem.Features.Auth.Login;
using MediatR;

namespace ExaminationSystem.Features.Quizzes.Commands.PublishQuiz
{
    public record PatchPublishQuizCommand(Guid QuizId) :  IRequest<Result<PublishQuizResponse>>;

}
