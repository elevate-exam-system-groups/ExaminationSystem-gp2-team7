using MediatR;

namespace ExaminationSystem.Features.Auth.ForgetPassword.OTP
{
    public class ResendOtpCommand: IRequest<string>
    {
        public string Email { get; set; }
    }
}
