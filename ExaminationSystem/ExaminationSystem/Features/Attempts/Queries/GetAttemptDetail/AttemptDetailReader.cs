using ExaminationSystem.Contracts;
using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Attempts.Queries.GetAttemptDetail
{
    
    public sealed class AttemptDetailReader
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttemptDetailReader(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       
        public async Task<AttemptMeta?> GetAttemptMetaAsync(
            Guid attemptId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<Attempt>().AsQueryable()
                .Where(a => a.Id == attemptId)
                .Select(a => new AttemptMeta
                {
                    StudentId = a.StudentId,
                    Status = a.Status
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

       
        public async Task<AttemptDetailResponse> GetAttemptSummaryAsync(
            Guid attemptId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<Attempt>().AsQueryable()
                .Where(a => a.Id == attemptId)
                .Select(a => new AttemptDetailResponse
                {
                    AttemptId = a.Id,
                    QuizTitle = a.Quiz.Title,
                    Score = a.Score ?? 0,
                    TotalQuestions = a.TotalQuestions ?? 0,
                    CorrectCount = a.CorrectAnswers ?? 0,
                    Passed = a.Passed ?? false,
                    Status = a.Status.ToString(),
                    SubmittedAt = a.SubmittedAt
                })
                .AsNoTracking()
                .FirstAsync(cancellationToken);
        }

      
        public async Task<List<AnswerProjection>> GetAnswersAsync(
            Guid attemptId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<Answer>().AsQueryable()
                .Where(a => a.AttemptId == attemptId)
                .OrderBy(a => a.Question.OrderIndex)
                .Select(a => new AnswerProjection
                {
                    QuestionId = a.QuestionId,
                    QuestionText = a.Question.QuestionText,
                    QuestionType = a.Question.QuestionType,
                    IsCorrect = a.IsCorrect,
                    StudentAnswer = a.StudentAnswer,
                    SelectedOptionId = a.SelectedOptionId,
                    SelectedOptionText = a.SelectedOption != null ? a.SelectedOption.OptionText : null
                })
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Dictionary<Guid, bool>> GetTfCorrectAnswersAsync(
            List<Guid> questionIds, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<TrueFalseQuestion>().AsQueryable()
                .Where(q => questionIds.Contains(q.Id))
                .Select(q => new { q.Id, q.CorrectAnswer })
                .AsNoTracking()
                .ToDictionaryAsync(q => q.Id, q => q.CorrectAnswer, cancellationToken);
        }

        
        public async Task<Dictionary<Guid, McqCorrectOption>> GetMcqCorrectOptionsAsync(
            List<Guid> questionIds, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<QuestionOption>().AsQueryable()
                .Where(o => questionIds.Contains(o.MCQQuestionId) && o.IsCorrect)
                .Select(o => new McqCorrectOption
                {
                    QuestionId = o.MCQQuestionId,
                    OptionId = o.Id,
                    OptionText = o.OptionText,
                    Explanation = o.Explanation
                })
                .AsNoTracking()
                .ToDictionaryAsync(o => o.QuestionId, cancellationToken);
        }
    }

   

    public class AttemptMeta
    {
        public Guid StudentId { get; set; }
        public AttemptStatus Status { get; set; }
    }

    public class AnswerProjection
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; } = default!;
        public string QuestionType { get; set; } = default!;
        public bool IsCorrect { get; set; }
        public bool? StudentAnswer { get; set; }
        public Guid? SelectedOptionId { get; set; }
        public string? SelectedOptionText { get; set; }
    }

    public class McqCorrectOption
    {
        public Guid QuestionId { get; set; }
        public Guid OptionId { get; set; }
        public string OptionText { get; set; } = default!;
        public string? Explanation { get; set; }
    }
}
