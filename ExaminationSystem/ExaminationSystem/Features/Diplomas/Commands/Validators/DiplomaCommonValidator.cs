using FluentValidation;


namespace ExaminationSystem.Features.Diplomas.Commands.Validators
{
    public class DiplomaCommonValidator<T> : AbstractValidator<T> where T : IDiplomaCommand
    {
        public DiplomaCommonValidator()
        {
            RuleFor(d => d.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters");

            RuleFor(d => d.TotalQuizCount)
                .InclusiveBetween(0, 30);
        }
    }
}
