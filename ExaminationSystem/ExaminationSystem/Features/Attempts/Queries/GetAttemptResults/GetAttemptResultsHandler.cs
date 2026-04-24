using ExaminationSystem.Common;
using ExaminationSystem.Features.Attempts.Queries.GetAttemptResults.Helpers;
using ExaminationSystem.Features.Attempts.Queries.ViewResults;
using ExaminationSystem.Models.Enums;
using MediatR;

namespace ExaminationSystem.Features.Attempts.Queries.GetAttemptResults
{
    public class GetAttemptResultsHandler
        : IRequestHandler<GetAttemptResultsQuery, Result<AttemptResultsResponse>>
    {
        private readonly AttemptResultsReader _reader;

        public GetAttemptResultsHandler(AttemptResultsReader reader)
        {
            _reader = reader;
        }

        public async Task<Result<AttemptResultsResponse>> Handle(
            GetAttemptResultsQuery request, CancellationToken cancellationToken)
        {
            // 1. Lightweight auth/status check
            var meta = await _reader.GetAttemptMetaAsync(request.AttemptId, cancellationToken);

            var validationError = Validate(meta, request);
            if (validationError is not null)
                return validationError;

            // 2. Full projection only after validation passes
            var attempt = await _reader.GetAttemptResultsAsync(request.AttemptId, cancellationToken);

            // 3. Fetch correct answers concurrently
            var questionIds = attempt.Answers.Select(a => a.QuestionId).Distinct().ToList();
            var (mcqLookup, tfLookup) = await _reader.GetCorrectAnswersAsync(questionIds, cancellationToken);

            // 4. Build and return response
            var answerDtos = AnswerDtoBuilder.Build(attempt.Answers, mcqLookup, tfLookup);
            return Result<AttemptResultsResponse>.Success(AttemptResultsMapper.ToResponse(attempt, answerDtos));
        }

        // ── Validation ─────────────────────────────────────────────────────────────

        private static Result<AttemptResultsResponse>? Validate(AttemptMeta? meta, GetAttemptResultsQuery request)
        {
            if (meta is null)
                return Result<AttemptResultsResponse>.Failure(
                    "Attempt not found.", StatusCodes.Status404NotFound);

            if (meta.StudentId != request.UserId && !request.IsAdmin)
                return Result<AttemptResultsResponse>.Failure(
                    "You do not have permission to view this attempt's results.", StatusCodes.Status403Forbidden);

            if (meta.Status == AttemptStatus.InProgress)
                return Result<AttemptResultsResponse>.Failure(
                    "Results are not available until the quiz is submitted.", StatusCodes.Status403Forbidden);

            return null;
        }
    }
}
