using MediatR;

namespace ExaminationSystem.Features.Auth.ForgetPassword.ResetPassword
{
    public class ResetPasswordCommand: IRequest<ResetPasswordResponse>
    {
        public string Email { get; set; }
        public string ResetToken { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
