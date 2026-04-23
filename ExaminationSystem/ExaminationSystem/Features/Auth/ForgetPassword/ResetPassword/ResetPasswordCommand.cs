using MediatR;

namespace ExaminationSystem.Features.Auth.ForgetPassword.ResetPassword
{
    public record ResetPasswordCommand : IRequest<string>
    {
        public string Email { get; init; }
        public string ResetToken { get; init; }
        public string NewPassword { get; init; }
        public string ConfirmPassword { get; init; }
    }
}
