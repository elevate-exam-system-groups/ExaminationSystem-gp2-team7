using ExaminationSystem.Contracts;
using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Attempts.SubmitQuiz.Helpers
{
    /// <summary>
    /// SRP: owns all database read/write operations needed by SubmitQuiz.
    /// No validation or orchestration logic lives here.
    /// </summary>
    public sealed class SubmitQuizReader
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubmitQuizReader(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// جيب المحاولة بالـ ID
        /// </summary>
        public async Task<Attempt?> GetAttemptAsync(
            Guid attemptId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<Attempt>().AsQueryable()
                .FirstOrDefaultAsync(a => a.Id == attemptId, cancellationToken);
        }

        /// <summary>
        /// جيب الكويز بالـ ID
        /// </summary>
        public async Task<Quiz?> GetQuizAsync(
            Guid quizId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<Quiz>().AsQueryable()
                .AsNoTracking()
                .FirstOrDefaultAsync(q => q.Id == quizId, cancellationToken);
        }

        /// <summary>
        /// عدد الأسئلة الكلي في الكويز
        /// </summary>
        public async Task<int> CountQuestionsAsync(
            Guid quizId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<Question>().AsQueryable()
                .CountAsync(q => q.QuizId == quizId, cancellationToken);
        }

        /// <summary>
        /// عدد الإجابات الصح (كل الإجابات)
        /// </summary>
        public async Task<int> CountCorrectAnswersAsync(
            Guid attemptId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<Answer>().AsQueryable()
                .CountAsync(a => a.AttemptId == attemptId && a.IsCorrect, cancellationToken);
        }

        /// <summary>
        /// عدد الإجابات الصح قبل الـ deadline (حالة TimedOut)
        /// </summary>
        public async Task<int> CountCorrectAnswersBeforeDeadlineAsync(
            Guid attemptId, DateTime deadline, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<Answer>().AsQueryable()
                .CountAsync(a =>
                    a.AttemptId == attemptId
                    && a.IsCorrect
                    && a.SubmittedAt <= deadline, cancellationToken);
        }

        /// <summary>
        /// احفظ كل التغييرات مرة واحدة
        /// </summary>
        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
