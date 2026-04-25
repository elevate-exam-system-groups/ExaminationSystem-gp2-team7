namespace ExaminationSystem.Contracts
{
    public interface IAttemptAutoSubmitService
    {
        Task AutoSubmitAsync(Guid attemptId, CancellationToken cancellationToken = default);
    }
}
