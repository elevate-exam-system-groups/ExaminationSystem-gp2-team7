using MediatR;

namespace ExaminationSystem.Features.Auth.ForgetPassword.OTP
{
    public record VerifyOtpCommand : IRequest<string>
    {
        public string Email { get; init; }
        public string Otp { get; init; }
    }
}
