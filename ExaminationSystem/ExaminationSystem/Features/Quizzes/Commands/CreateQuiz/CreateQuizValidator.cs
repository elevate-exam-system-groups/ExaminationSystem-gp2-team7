using FluentValidation;

namespace ExaminationSystem.Features.Quizzes.Commands.CreateQuiz
{
    public class CreateQuizValidator : AbstractValidator<CreateQuizCommand>
    {
        public CreateQuizValidator()
        {
            RuleFor(q => q.Title)
                .NotEmpty().WithMessage("Title is required.");

            RuleFor(q => q.DurationMinutes)
                .NotEmpty().WithMessage("Duration in minutes is required.")
                .GreaterThan(0).WithMessage("Duration must be a positive integer.");

            RuleFor(q => q.PassScore)
                .InclusiveBetween(0, 100).WithMessage("Pass score must be between 0 and 100.");

            RuleFor(q => q.DiplomaId)
                .NotEmpty().WithMessage("Diploma ID is required.");
        }
    }
}
