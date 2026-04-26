using ExaminationSystem.Common;
using MediatR;

namespace ExaminationSystem.Features.Auth.ForgetPassword.OTP
{
    public record SendOtpCommand : IRequest<Result<string>>
    {
        public string Email { get; init; }
    }
}
