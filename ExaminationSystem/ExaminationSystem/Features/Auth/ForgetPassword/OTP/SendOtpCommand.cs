using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Features.Auth.ForgetPassword.OTP
{
    public class SendOtpCommand: IRequest<string>
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
    }
}
