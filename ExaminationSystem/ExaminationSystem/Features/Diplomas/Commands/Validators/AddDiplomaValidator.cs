using FluentValidation;

namespace ExaminationSystem.Features.Diplomas.Commands.Validators
{
    public class AddDiplomaValidator : AbstractValidator<AddDiplomaCommand>
    {
        public AddDiplomaValidator()
        {
            Include(new DiplomaCommonValidator<AddDiplomaCommand>());
        }
    }
}
