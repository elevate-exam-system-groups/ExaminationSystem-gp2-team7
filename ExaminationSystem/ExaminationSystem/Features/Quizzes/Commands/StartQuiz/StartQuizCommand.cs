using ExaminationSystem.Common;
using MediatR;

namespace ExaminationSystem.Features.Quizzes.Commands.StartQuiz
{
    public record StartQuizCommand(Guid QuizId, Guid StudentId) : IRequest<Result<StartQuizResponse>>;
}
