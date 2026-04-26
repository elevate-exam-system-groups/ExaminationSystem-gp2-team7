using ExaminationSystem.Common;
using MediatR;

namespace ExaminationSystem.Features.Auth.ForgetPassword.OTP
{
    public record VerifyOtpCommand : IRequest<Result<string>>
    {
        public string Email { get; init; }
        public string Otp { get; init; }
    }
}
