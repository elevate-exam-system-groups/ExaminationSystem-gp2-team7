using System.Text.Json.Serialization;

namespace ExaminationSystem.Features.Attempts.Queries.GetAttemptDetail
{
    
    public record AttemptDetailResponse
    {
        public Guid AttemptId { get; init; }
        public string QuizTitle { get; init; } = default!;
        public decimal Score { get; init; }
        public int TotalQuestions { get; init; }
        public int CorrectCount { get; init; }
        public bool Passed { get; init; }
        public string Status { get; init; } = default!;
        public DateTime? SubmittedAt { get; init; }
        public List<AnswerDetailDto> Answers { get; init; } = [];
    }

    
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "questionType")]
    [JsonDerivedType(typeof(TrueFalseAnswerDto), "TrueFalse")]
    [JsonDerivedType(typeof(McqAnswerDto), "MCQ")]
    public abstract record AnswerDetailDto
    {
        public Guid QuestionId { get; init; }
        public string QuestionText { get; init; } = default!;
        public bool IsCorrect { get; init; }
    }

   
    public record TrueFalseAnswerDto : AnswerDetailDto
    {
        public bool StudentAnswer { get; init; }
        public bool CorrectAnswer { get; init; }
    }

    
    public record McqAnswerDto : AnswerDetailDto
    {
        public Guid? SelectedOptionId { get; init; }
        public string? SelectedOptionText { get; init; }
        public Guid CorrectOptionId { get; init; }
        public string CorrectOptionText { get; init; } = default!;
        public string? Explanation { get; init; }
    }
}
