namespace ExaminationSystem.Features.Attempts.Queries.GetAttemptResults.Helpers
{
    /// <summary>
    /// Maps projection data to the final API response DTO.
    /// </summary>
    public static class AttemptResultsMapper
    {
        public static AttemptResultsResponse ToResponse(
            AttemptProjection attempt, List<AnswerResultDto> answers)
        {
            return new AttemptResultsResponse
            {
                AttemptId = attempt.Id,
                QuizTitle = attempt.QuizTitle,
                Score = attempt.Score ?? 0,
                TotalQuestions = attempt.TotalQuestions ?? 0,
                CorrectCount = attempt.CorrectAnswers ?? 0,
                Passed = attempt.Passed ?? false,
                PassScore = attempt.PassScore,
                SubmittedAt = attempt.SubmittedAt,
                Answers = answers
            };
        }
    }
}
