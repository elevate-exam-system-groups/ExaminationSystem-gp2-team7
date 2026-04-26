using ExaminationSystem.Contracts;
using ExaminationSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Questions.Commands.DeleteQuestion.Helpers
{
    
    public sealed class DeleteQuestionReader
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteQuestionReader(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       
        public async Task<Question?> GetQuestionAsync(
            Guid questionId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<Question>().AsQueryable()
                .FirstOrDefaultAsync(q => q.Id == questionId, cancellationToken);
        }

       
        public async Task<Quiz?> GetQuizAsync(
            Guid quizId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<Quiz>().AsQueryable()
                .FirstOrDefaultAsync(q => q.Id == quizId, cancellationToken);
        }

       
        public void SoftDeleteQuestion(Question question)
        {
            _unitOfWork.GetRepository<Question>().SoftDelete(question);
        }

        public async Task SoftDeleteOptionsAsync(
            Guid questionId, CancellationToken cancellationToken)
        {
            var options = await _unitOfWork.GetRepository<QuestionOption>().AsQueryable()
                .Where(o => o.MCQQuestionId == questionId)
                .ToListAsync(cancellationToken);

            var optionRepo = _unitOfWork.GetRepository<QuestionOption>();
            foreach (var option in options)
                optionRepo.SoftDelete(option);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
