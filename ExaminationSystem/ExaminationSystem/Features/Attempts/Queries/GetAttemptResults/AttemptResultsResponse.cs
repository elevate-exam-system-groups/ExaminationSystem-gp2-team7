using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Features.Attempts.Queries.GetAttemptResults
{
    /// <summary>
    /// Top-level response for viewing attempt results.
    /// </summary>
    public record AttemptResultsResponse
    {
        public Guid AttemptId { get; init; }
        public string QuizTitle { get; init; } = default!;
        public decimal Score { get; init; }
        public int TotalQuestions { get; init; }
        public int CorrectCount { get; init; }
        public bool Passed { get; init; }
        public decimal PassScore { get; init; }
        public DateTime? SubmittedAt { get; init; }
        public List<AnswerResultDto> Answers { get; init; } = [];
    }

    /// <summary>
    /// Per-question answer detail with correct answer revealed post-submission.
    /// Minimum 2 answer choices; exactly one must be marked correct.
    /// </summary>
    public record AnswerResultDto
    {
        public Guid QuestionId { get; init; }
        public string QuestionText { get; init; } = default!;
        public string QuestionType { get; init; } = default!;
        public bool IsCorrect { get; init; }

        // MCQ fields (exactly one correct option per question)
        public Guid? SelectedOptionId { get; init; }
        public string? SelectedOptionText { get; init; }
        public Guid? CorrectOptionId { get; init; }
        public string? CorrectOptionText { get; init; }

        // True/False fields
        public bool? StudentAnswer { get; init; }
        public bool? CorrectAnswer { get; init; }

        // Explanation (from the correct option if available)
        public string? Explanation { get; init; }
    }
}


