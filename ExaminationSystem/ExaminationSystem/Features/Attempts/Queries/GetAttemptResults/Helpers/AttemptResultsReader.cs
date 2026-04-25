using ExaminationSystem.Contracts;
using ExaminationSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Attempts.Queries.GetAttemptResults.Helpers
{
    /// <summary>
    /// SRP: owns all database read operations needed to build attempt results.
    /// No validation or mapping logic lives here.
    /// </summary>
    public sealed class AttemptResultsReader
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttemptResultsReader(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Lightweight metadata fetch for authorization and status checks.
        /// </summary>
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
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Full projection of attempt results with answers.
        /// </summary>
        public async Task<AttemptProjection> GetAttemptResultsAsync(
            Guid attemptId, CancellationToken cancellationToken)
        {
            return (await _unitOfWork.GetRepository<Attempt>().AsQueryable()
                .Where(a => a.Id == attemptId)
                .Select(a => new AttemptProjection
                {
                    Id = a.Id,
                    Score = a.Score,
                    TotalQuestions = a.TotalQuestions,
                    CorrectAnswers = a.CorrectAnswers,
                    Passed = a.Passed,
                    SubmittedAt = a.SubmittedAt,
                    QuizTitle = a.Quiz.Title,
                    PassScore = a.Quiz.PassScore,
                    Answers = a.Answers.Select(ans => new AnswerProjection
                    {
                        QuestionId = ans.QuestionId,
                        QuestionText = ans.Question.QuestionText,
                        QuestionType = ans.Question.QuestionType,
                        IsCorrect = ans.IsCorrect,
                        SelectedOptionId = ans.SelectedOptionId,
                        SelectedOptionText = ans.SelectedOption != null ? ans.SelectedOption.OptionText : null,
                        StudentAnswer = ans.StudentAnswer
                    }).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken))!;
        }

        /// <summary>
        /// Fetches correct MCQ options and True/False answers concurrently.
        /// </summary>
        public async Task<(Dictionary<Guid, CorrectOptionProjection> McqLookup, Dictionary<Guid, bool> TfLookup)>
            GetCorrectAnswersAsync(List<Guid> questionIds, CancellationToken cancellationToken)
        {
            var mcqTask = GetCorrectOptionsAsync(questionIds, cancellationToken);
            var tfTask = GetTrueFalseAnswersAsync(questionIds, cancellationToken);

            await Task.WhenAll(mcqTask, tfTask);

            return (await mcqTask, await tfTask);
        }

        private async Task<Dictionary<Guid, CorrectOptionProjection>> GetCorrectOptionsAsync(
            List<Guid> questionIds, CancellationToken cancellationToken)
        {
            var options = await _unitOfWork.GetRepository<Option>().AsQueryable()
                .Where(o => questionIds.Contains(o.MCQQuestionId) && o.IsCorrect)
                .Select(o => new CorrectOptionProjection
                {
                    QuestionId = o.MCQQuestionId,
                    OptionId = o.Id,
                    OptionText = o.OptionText,
                    Explanation = o.Explanation
                })
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return options
                .GroupBy(o => o.QuestionId)
                .ToDictionary(g => g.Key, g => g.First());
        }

        private async Task<Dictionary<Guid, bool>> GetTrueFalseAnswersAsync(
            List<Guid> questionIds, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<TrueFalseQuestion>().AsQueryable()
                .Where(q => questionIds.Contains(q.Id))
                .Select(q => new { q.Id, q.CorrectAnswer })
                .AsNoTracking()
                .ToDictionaryAsync(q => q.Id, q => q.CorrectAnswer, cancellationToken);
        }
    }
}
