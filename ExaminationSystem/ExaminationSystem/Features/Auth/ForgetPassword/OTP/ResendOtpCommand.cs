using MediatR;

namespace ExaminationSystem.Features.Auth.ForgetPassword.OTP
{
    public record ResendOtpCommand : IRequest<string>
    {
        public string Email { get; init; }
    }
}
