using ExaminationSystem.Models;
using FluentValidation;

namespace ExaminationSystem.Features.Diplomas.Commands.Validators
{
    public class UpdateDiplomaValidator : AbstractValidator<UpdateDiplomaCommand>
    {
        public UpdateDiplomaValidator() 
        {
            Include(new DiplomaCommonValidator<UpdateDiplomaCommand>());

            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required");
        }
    }
}
