using ExaminationSystem.Common;
using MediatR;

namespace ExaminationSystem.Features.Attempts.SubmitQuiz
{
    public record SubmitQuizCommand : IRequest<SubmitQuizResponse>
    {
        public Guid AttemptId { get; init; }

        public Guid StudentId { get; init; }
    }
}
