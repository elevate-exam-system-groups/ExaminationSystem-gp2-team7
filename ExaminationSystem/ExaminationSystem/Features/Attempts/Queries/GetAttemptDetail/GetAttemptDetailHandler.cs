using ExaminationSystem.Common;
using ExaminationSystem.Common.Constants;
using ExaminationSystem.Models.Enums;
using MediatR;

namespace ExaminationSystem.Features.Attempts.Queries.GetAttemptDetail
{
    public class GetAttemptDetailHandler
        : IRequestHandler<GetAttemptDetailQuery, Result<AttemptDetailResponse>>
    {
        private readonly AttemptDetailReader _reader;

        public GetAttemptDetailHandler(AttemptDetailReader reader)
        {
            _reader = reader;
        }

        public async Task<Result<AttemptDetailResponse>> Handle(
            GetAttemptDetailQuery request, CancellationToken cancellationToken)
        {
           
            var meta = await _reader.GetAttemptMetaAsync(request.AttemptId, cancellationToken);

            if (meta is null)
                return Result<AttemptDetailResponse>.Failure(
                    "Attempt not found.", StatusCodes.Status404NotFound);

            if (meta.StudentId != request.StudentId)
                return Result<AttemptDetailResponse>.Failure(
                    "You do not have permission to view this attempt.", StatusCodes.Status403Forbidden);

            if (meta.Status == AttemptStatus.InProgress)
                return Result<AttemptDetailResponse>.Failure(
                    "Results are not available until the quiz is submitted.", StatusCodes.Status403Forbidden);

            
            var response = await _reader.GetAttemptSummaryAsync(request.AttemptId, cancellationToken);

           
            var answers = await _reader.GetAnswersAsync(request.AttemptId, cancellationToken);

           
            var questionIds = answers.Select(a => a.QuestionId).Distinct().ToList();
            var tfCorrectAnswers = await _reader.GetTfCorrectAnswersAsync(questionIds, cancellationToken);
            var mcqCorrectOptions = await _reader.GetMcqCorrectOptionsAsync(questionIds, cancellationToken);

           
            response = response with
            {
                Answers = answers.Select(a =>
                {
                    AnswerDetailDto dto;

                    if (a.QuestionType == QuestionTypes.TrueFalse)
                    {
                        dto = new TrueFalseAnswerDto
                        {
                            QuestionId = a.QuestionId,
                            QuestionText = a.QuestionText,
                            IsCorrect = a.IsCorrect,
                            StudentAnswer = a.StudentAnswer ?? false,
                            CorrectAnswer = tfCorrectAnswers.GetValueOrDefault(a.QuestionId)
                        };
                    }
                    else
                    {
                        var correctOpt = mcqCorrectOptions.GetValueOrDefault(a.QuestionId);
                        dto = new McqAnswerDto
                        {
                            QuestionId = a.QuestionId,
                            QuestionText = a.QuestionText,
                            IsCorrect = a.IsCorrect,
                            SelectedOptionId = a.SelectedOptionId,
                            SelectedOptionText = a.SelectedOptionText,
                            CorrectOptionId = correctOpt?.OptionId ?? Guid.Empty,
                            CorrectOptionText = correctOpt?.OptionText ?? "",
                            Explanation = correctOpt?.Explanation
                        };
                    }

                    return dto;
                }).ToList()
            };

            return Result<AttemptDetailResponse>.Success(response);
        }
    }
}
