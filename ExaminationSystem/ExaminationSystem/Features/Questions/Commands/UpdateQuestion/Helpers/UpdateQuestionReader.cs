using ExaminationSystem.Contracts;
using ExaminationSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Questions.Commands.UpdateQuestion.Helpers
{
    /// <summary>
    /// SRP: owns all database operations needed by UpdateQuestion.
    /// </summary>
    public sealed class UpdateQuestionReader
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateQuestionReader(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// جيب السؤال MCQ بالـ ID
        /// </summary>
        public async Task<MultipleChoiceQuestion?> GetQuestionAsync(
            Guid questionId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<MultipleChoiceQuestion>().AsQueryable()
                .FirstOrDefaultAsync(q => q.Id == questionId, cancellationToken);
        }

        /// <summary>
        /// جيب الـ Options القديمة بتاعت السؤال
        /// </summary>
        public async Task<List<QuestionOption>> GetOptionsByQuestionIdAsync(
            Guid questionId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<QuestionOption>().AsQueryable()
                .Where(o => o.MCQQuestionId == questionId)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// امسح الـ Options القديمة
        /// </summary>
        public void RemoveOptions(List<QuestionOption> options)
        {
            var optionRepo = _unitOfWork.GetRepository<QuestionOption>();
            foreach (var option in options)
                optionRepo.HardDelete(option);
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
