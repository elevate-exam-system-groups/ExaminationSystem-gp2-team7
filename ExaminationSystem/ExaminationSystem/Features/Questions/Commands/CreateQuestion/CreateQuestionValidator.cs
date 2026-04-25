using FluentValidation;

namespace ExaminationSystem.Features.Questions.Commands.CreateQuestion
{
    public class CreateQuestionValidator : AbstractValidator<CreateQuestionCommand>
    {
        public CreateQuestionValidator()
        {
            // نص السؤال مطلوب
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Question text is required.");

            // لازم 2 اختيارات على الأقل
            RuleFor(x => x.Options)
                .Must(options => options != null && options.Count >= 2)
                .WithMessage("At least 2 options are required.");

            // لازم واحد بالظبط يكون correct
            RuleFor(x => x.Options)
                .Must(options => options != null && options.Count(o => o.IsCorrect) == 1)
                .WithMessage("Exactly one correct option required.");

            // كل اختيار لازم يكون فيه نص
            RuleForEach(x => x.Options).ChildRules(option =>
            {
                option.RuleFor(o => o.Text)
                    .NotEmpty().WithMessage("Option text is required.");
            });
        }
    }
}
