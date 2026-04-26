using ExaminationSystem.Features.Questions.Commands.CreateQuestion;
using FluentValidation;

namespace ExaminationSystem.Features.Questions.Commands.UpdateQuestion
{
    public class UpdateQuestionValidator : AbstractValidator<UpdateQuestionCommand>
    {
        public UpdateQuestionValidator()
        {
            
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Question text is required.");

           
            When(x => x.Options != null && x.Options.Any(), () =>
            {
                RuleFor(x => x.Options)
                    .Must(options => options!.Count >= 2)
                    .WithMessage("At least 2 options are required.");

                RuleFor(x => x.Options)
                    .Must(options => options!.Count(o => o.IsCorrect) == 1)
                    .WithMessage("Exactly one correct option required.");

                RuleForEach(x => x.Options).ChildRules(option =>
                {
                    option.RuleFor(o => o.Text)
                        .NotEmpty().WithMessage("Option text is required.");
                });
            });
        }
    }
}
