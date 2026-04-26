using ExaminationSystem.Contracts;
using ExaminationSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Questions.Commands.CreateQuestion.Helpers
{
   
    public sealed class CreateQuestionReader
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateQuestionReader(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

      
        public async Task<Quiz?> GetQuizAsync(Guid quizId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<Quiz>().AsQueryable()
                .FirstOrDefaultAsync(q => q.Id == quizId, cancellationToken);
        }

        public async Task<int> GetLastOrderIndexAsync(Guid quizId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<Question>().AsQueryable()
                .Where(q => q.QuizId == quizId)
                .MaxAsync(q => (int?)q.OrderIndex, cancellationToken) ?? 0;
        }

       
        public void AddQuestion(Question question)
        {
            if (question is MultipleChoiceQuestion mcq)
                _unitOfWork.GetRepository<MultipleChoiceQuestion>().Add(mcq);
            else if (question is TrueFalseQuestion tf)
                _unitOfWork.GetRepository<TrueFalseQuestion>().Add(tf);
        }

       
        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
