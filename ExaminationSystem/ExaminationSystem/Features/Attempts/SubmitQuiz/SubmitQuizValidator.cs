using FluentValidation;

namespace ExaminationSystem.Features.Attempts.SubmitQuiz
{
    public class SubmitQuizValidator: AbstractValidator<SubmitQuizCommand>
    {
        public SubmitQuizValidator()
        { 
            RuleFor(x => x.AttemptId).NotEmpty().WithMessage("AttemptId is required.");

            RuleFor(x => x.StudentId).NotEmpty().WithMessage("StudentId is required.");

        }
    }
}
