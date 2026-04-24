using ExaminationSystem.Features.Attempts.Queries.ViewResults;
using FluentValidation;

namespace ExaminationSystem.Features.Attempts.Queries.GetAttemptResults
{
    public class GetAttemptResultsValidator : AbstractValidator<GetAttemptResultsQuery>
    {
        public GetAttemptResultsValidator()
        {
            RuleFor(x => x.AttemptId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
