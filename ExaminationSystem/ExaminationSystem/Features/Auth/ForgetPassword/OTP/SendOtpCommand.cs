using MediatR;

namespace ExaminationSystem.Features.Auth.ForgetPassword.OTP
{
    public record SendOtpCommand : IRequest<string>
    {
        public string Email { get; init; }
    }
}
