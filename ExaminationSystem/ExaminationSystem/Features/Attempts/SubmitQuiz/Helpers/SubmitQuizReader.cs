using ExaminationSystem.Contracts;
using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Attempts.SubmitQuiz.Helpers
{
  
    public sealed class SubmitQuizReader
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubmitQuizReader(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       
        public async Task<Attempt?> GetAttemptAsync(
            Guid attemptId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<Attempt>().AsQueryable()
                .FirstOrDefaultAsync(a => a.Id == attemptId, cancellationToken);
        }

       
        public async Task<Quiz?> GetQuizAsync(
            Guid quizId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<Quiz>().AsQueryable()
                .AsNoTracking()
                .FirstOrDefaultAsync(q => q.Id == quizId, cancellationToken);
        }

      
        public async Task<int> CountQuestionsAsync(
            Guid quizId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<Question>().AsQueryable()
                .CountAsync(q => q.QuizId == quizId, cancellationToken);
        }

       
        public async Task<int> CountCorrectAnswersAsync(
            Guid attemptId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<Answer>().AsQueryable()
                .CountAsync(a => a.AttemptId == attemptId && a.IsCorrect, cancellationToken);
        }

      
        public async Task<int> CountCorrectAnswersBeforeDeadlineAsync(
            Guid attemptId, DateTime deadline, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<Answer>().AsQueryable()
                .CountAsync(a =>
                    a.AttemptId == attemptId
                    && a.IsCorrect
                    && a.SubmittedAt <= deadline, cancellationToken);
        }

       
        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
