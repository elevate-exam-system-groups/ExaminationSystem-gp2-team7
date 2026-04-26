using FluentValidation;

namespace ExaminationSystem.Features.Questions.Commands.CreateQuestion
{
    public class CreateQuestionValidator : AbstractValidator<CreateQuestionCommand>
    {
        private static readonly string[] AllowedTypes = { "MCQ", "TrueFalse" };

        public CreateQuestionValidator()
        {
            
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Question text is required.");

           
            RuleFor(x => x.QuestionType)
                .NotEmpty().WithMessage("Question type is required.")
                .Must(t => AllowedTypes.Contains(t))
                .WithMessage("Question type must be 'MCQ' or 'TrueFalse'.");

            // ===== MCQ Validation =====
            When(x => x.QuestionType == "MCQ", () =>
            {
               
                RuleFor(x => x.Options)
                    .NotNull().WithMessage("Options are required for MCQ questions.")
                    .Must(options => options != null && options.Count >= 2)
                    .WithMessage("At least 2 options are required.");

                
                RuleFor(x => x.Options)
                    .Must(options => options != null && options.Count(o => o.IsCorrect) == 1)
                    .WithMessage("Exactly one correct option required.");

             
                RuleForEach(x => x.Options).ChildRules(option =>
                {
                    option.RuleFor(o => o.Text)
                        .NotEmpty().WithMessage("Option text is required.");
                });
            });

            // ===== TrueFalse Validation =====
            When(x => x.QuestionType == "TrueFalse", () =>
            {
                RuleFor(x => x.CorrectAnswer)
                    .NotNull().WithMessage("Correct answer is required for TrueFalse questions.");
            });
        }
    }
}
