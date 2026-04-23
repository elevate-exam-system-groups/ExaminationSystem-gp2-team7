using FluentValidation;

namespace ExaminationSystem.Features.Auth.ForgetPassword.OTP
{
    public class SendOtpValidator : AbstractValidator<SendOtpCommand>
    {
        public SendOtpValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
