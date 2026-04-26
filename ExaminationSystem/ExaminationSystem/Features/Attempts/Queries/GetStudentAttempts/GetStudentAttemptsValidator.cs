using FluentValidation;

namespace ExaminationSystem.Features.Attempts.Queries.GetStudentAttempts
{
    public class GetStudentAttemptsValidator : AbstractValidator<GetStudentAttemptsQuery>
    {
        public GetStudentAttemptsValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0)
                .WithMessage("Page must be greater than 0");

            RuleFor(x => x.PerPage)
                .InclusiveBetween(1, 50)
                .WithMessage("PerPage must be between 1 and 50");

            RuleFor(x => x.SortBy)
                .Must(x => x == null ||
                           x.ToLower() == "submitted_at" ||
                           x.ToLower() == "score")
                .WithMessage("SortBy must be 'submitted_at' or 'score'");

            RuleFor(x => x.Order)
                .Must(x => x == null ||
                           x.ToLower() == "asc" ||
                           x.ToLower() == "desc")
                .WithMessage("Order must be 'asc' or 'desc'");
        }
    }
}
