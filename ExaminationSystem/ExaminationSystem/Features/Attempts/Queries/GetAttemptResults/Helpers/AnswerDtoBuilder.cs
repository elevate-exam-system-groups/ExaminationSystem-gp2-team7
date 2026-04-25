using ExaminationSystem.Common;
using ExaminationSystem.Common.Constants;

namespace ExaminationSystem.Features.Attempts.Queries.GetAttemptResults.Helpers
{
    /// <summary>
    /// Builds per-question answer DTOs by enriching student answers
    /// with the correct answer data from MCQ and True/False lookups.
    /// </summary>
    public static class AnswerDtoBuilder
    {
        public static List<AnswerResultDto> Build(
            List<AnswerProjection> answers,
            Dictionary<Guid, CorrectOptionProjection> mcqLookup,
            Dictionary<Guid, bool> tfLookup)
        {
            return answers.ConvertAll(answer =>
            {
                var dto = new AnswerResultDto
                {
                    QuestionId = answer.QuestionId,
                    QuestionText = answer.QuestionText,
                    QuestionType = answer.QuestionType,
                    IsCorrect = answer.IsCorrect
                };

                if (answer.QuestionType == QuestionTypes.MCQ)
                {
                    dto = dto with
                    {
                        SelectedOptionId = answer.SelectedOptionId,
                        SelectedOptionText = answer.SelectedOptionText,
                        CorrectOptionId = mcqLookup.TryGetValue(answer.QuestionId, out var correct)
                            ? correct.OptionId : null,
                        CorrectOptionText = correct?.OptionText,
                        Explanation = correct?.Explanation
                    };
                }
                else
                {
                    dto = dto with
                    {
                        StudentAnswer = answer.StudentAnswer,
                        CorrectAnswer = tfLookup.GetValueOrDefault(answer.QuestionId)
                    };
                }

                return dto;
            });
        }
    }
}
