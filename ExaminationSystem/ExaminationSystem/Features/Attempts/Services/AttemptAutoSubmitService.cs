using ExaminationSystem.Contracts;
using ExaminationSystem.Models;
using Microsoft.EntityFrameworkCore;
using ExaminationSystem.Models.Enums;

namespace ExaminationSystem.Features.Attempts.Services
{
    public sealed class AttemptAutoSubmitService(IUnitOfWork unitOfWork) : IAttemptAutoSubmitService
    {
        public async Task AutoSubmitAsync(Guid attemptId, CancellationToken cancellationToken = default)
        {
            var attemptRepo = unitOfWork.GetRepository<Attempt>();

            // Load the attempt with necessary navigation properties
            var attemptData = await attemptRepo.AsQueryable()
                .Where(a => a.Id == attemptId && a.Status == AttemptStatus.InProgress)
                .Select(a => new
                {
                    TotalQuestions = a.Quiz.TotalQuestionsCache,
                    PassScore = a.Quiz.PassScore,
                    CorrectAnswersCount = a.Answers.Count(a => a.IsCorrect)
                })
                .FirstOrDefaultAsync(cancellationToken);

            // If it doesn't exist or isn't InProgress, do nothing
            if(attemptData is null) return;

            // Calculate Score
            var score = attemptData.TotalQuestions > 0 ?
                Math.Round(((decimal)attemptData.CorrectAnswersCount / attemptData.TotalQuestions) * 100M, 2)
                : 0M;

            // Attach a stub entity to the Change Tracker
            var attemptToUpdate = new Attempt { Id = attemptId };
            attemptRepo.Update(attemptToUpdate);

            // Update the specific properties
            attemptToUpdate.Status = AttemptStatus.TimedOut;
            attemptToUpdate.SubmittedAt = DateTime.UtcNow;
            attemptToUpdate.Score = score;
            attemptToUpdate.Passed = score >= attemptData.PassScore;

            // Persist via UnitOfWork
            await unitOfWork.SaveChangesAsync(cancellationToken);

        }
    }
}
