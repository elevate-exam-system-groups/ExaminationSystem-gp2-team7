using ExaminationSystem.Contracts;
using ExaminationSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Questions.Commands.UpdateQuestion.Helpers
{
    public sealed class UpdateQuestionReader
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateQuestionReader(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string?> GetQuestionTypeAsync(
            Guid questionId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<Question>().AsQueryable()
                .Where(q => q.Id == questionId)
                .Select(q => q.QuestionType)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<TrueFalseQuestion?> GetTrueFalseQuestionAsync(
            Guid questionId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<TrueFalseQuestion>().AsQueryable()
                .FirstOrDefaultAsync(q => q.Id == questionId, cancellationToken);
        }

        public async Task<MultipleChoiceQuestion?> GetMcqQuestionAsync(
            Guid questionId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<MultipleChoiceQuestion>().AsQueryable()
                .FirstOrDefaultAsync(q => q.Id == questionId, cancellationToken);
        }

        public async Task<List<QuestionOption>> GetOptionsByQuestionIdAsync(
            Guid questionId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<QuestionOption>().AsQueryable()
                .Where(o => o.MCQQuestionId == questionId)
                .ToListAsync(cancellationToken);
        }

        public void RemoveOptions(List<QuestionOption> options)
        {
            var optionRepo = _unitOfWork.GetRepository<QuestionOption>();
            foreach (var option in options)
                optionRepo.SoftDelete(option);
        }

        public void AddOption(QuestionOption option)
        {
            _unitOfWork.GetRepository<QuestionOption>().Add(option);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
