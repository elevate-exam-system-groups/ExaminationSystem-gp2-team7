using ExaminationSystem.Contracts;
using ExaminationSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Questions.Commands.DeleteQuestion.Helpers
{
    /// <summary>
    /// SRP: owns all database operations needed by DeleteQuestion.
    /// </summary>
    public sealed class DeleteQuestionReader
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteQuestionReader(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// جيب السؤال بالـ ID
        /// </summary>
        public async Task<Question?> GetQuestionAsync(
            Guid questionId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<Question>().AsQueryable()
                .FirstOrDefaultAsync(q => q.Id == questionId, cancellationToken);
        }

        /// <summary>
        /// جيب الكويز بتاع السؤال (عشان نشوف لو Published)
        /// </summary>
        public async Task<Quiz?> GetQuizAsync(
            Guid quizId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<Quiz>().AsQueryable()
                .FirstOrDefaultAsync(q => q.Id == quizId, cancellationToken);
        }

        /// <summary>
        /// Soft delete السؤال
        /// </summary>
        public void SoftDeleteQuestion(Question question)
        {
            _unitOfWork.GetRepository<Question>().SoftDelete(question);
        }

        /// <summary>
        /// Soft delete كل الـ Options بتاعت السؤال
        /// </summary>
        public async Task SoftDeleteOptionsAsync(
            Guid questionId, CancellationToken cancellationToken)
        {
            var options = await _unitOfWork.GetRepository<Option>().AsQueryable()
                .Where(o => o.MCQQuestionId == questionId)
                .ToListAsync(cancellationToken);

            var optionRepo = _unitOfWork.GetRepository<Option>();
            foreach (var option in options)
                optionRepo.SoftDelete(option);
        }

        /// <summary>
        /// احفظ كل التغييرات
        /// </summary>
        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
