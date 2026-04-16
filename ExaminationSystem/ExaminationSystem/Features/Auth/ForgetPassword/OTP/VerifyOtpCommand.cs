using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Features.Auth.ForgetPassword.OTP
{
    public class VerifyOtpCommand: IRequest<string>
    {
        public string Email { get; set; }
        public string Otp { get; set; }

    }
}
