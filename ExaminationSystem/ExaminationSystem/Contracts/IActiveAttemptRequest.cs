using MediatR;

namespace ExaminationSystem.Contracts
{
    public interface IActiveAttemptRequest<TResponse> : IRequest<TResponse>
    {
        public Guid AttemptId { get; }
        TResponse CreateTimedOutResponse();

    }
}
