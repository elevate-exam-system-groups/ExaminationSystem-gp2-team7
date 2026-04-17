using FluentValidation;
using System.Data;

namespace ExaminationSystem.Features.Quizzes.Commands.UpdateQuiz
{
    public class UpdateQuizValidator : AbstractValidator<UpdateQuizCommand>
    {
        public UpdateQuizValidator() {

            RuleFor(q => q.Title)
                .NotEmpty().WithMessage("Title is required.");

            RuleFor(q => q.DurationMinutes)
                .NotEmpty().WithMessage("Duration in minutes is required.")
                .GreaterThan(0).WithMessage("Duration must be a positive integer.");

            RuleFor(q => q.PassScore)
                .InclusiveBetween(0, 100).WithMessage("Pass score must be between 0 and 100.");

        }
    }
}
