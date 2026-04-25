using ExaminationSystem.Contracts;
using ExaminationSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Questions.Commands.CreateQuestion.Helpers
{
    /// <summary>
    /// SRP: owns all database operations needed by CreateQuestion.
    /// No validation or orchestration logic lives here.
    /// </summary>
    public sealed class CreateQuestionReader
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateQuestionReader(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// جيب الكويز بالـ ID
        /// </summary>
        public async Task<Quiz?> GetQuizAsync(Guid quizId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<Quiz>().AsQueryable()
                .FirstOrDefaultAsync(q => q.Id == quizId, cancellationToken);
        }

        /// <summary>
        /// جيب آخر OrderIndex في الكويز (عشان السؤال الجديد يتحط بعده)
        /// </summary>
        public async Task<int> GetLastOrderIndexAsync(Guid quizId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<Question>().AsQueryable()
                .Where(q => q.QuizId == quizId)
                .MaxAsync(q => (int?)q.OrderIndex, cancellationToken) ?? 0;
        }

        /// <summary>
        /// اضيف السؤال في الداتابيز
        /// </summary>
        public void AddQuestion(MultipleChoiceQuestion question)
        {
            _unitOfWork.GetRepository<MultipleChoiceQuestion>().Add(question);
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
