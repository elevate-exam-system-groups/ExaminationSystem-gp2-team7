using ExaminationSystem.Common;
using MediatR;

namespace ExaminationSystem.Features.Auth.ForgetPassword.OTP
{
    public record ResendOtpCommand : IRequest<Result<string>>
    {
        public string Email { get; init; }
    }
}
