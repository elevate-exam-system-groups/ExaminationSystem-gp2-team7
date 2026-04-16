using ExaminationSystem.Models;
using ExaminationSystem.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Features.Auth.ForgetPassword.ResetPassword
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, ResetPasswordResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ResetPasswordResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            
            if (string.IsNullOrEmpty(request.Email))
                return new ResetPasswordResponse { Success = false, Message = "Email is required" };

            if (string.IsNullOrEmpty(request.NewPassword))
                return new ResetPasswordResponse { Success = false, Message = "Password is required" };

            if (request.NewPassword != request.ConfirmPassword)
                return new ResetPasswordResponse { Success = false, Message = "Passwords do not match" };

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return new ResetPasswordResponse { Success = false, Message = "Invalid request" };

            
            var tokenHash = OtpHelper.HashOtp(request.ResetToken);
            if (user.ResetToken != tokenHash)
                return new ResetPasswordResponse { Success = false, Message = "Token invalid or expired" };

           
            if (user.ResetTokenExpiresAt == null || user.ResetTokenExpiresAt < DateTime.UtcNow)
                return new ResetPasswordResponse { Success = false, Message = "Token invalid or expired" };

           
            await _userManager.RemovePasswordAsync(user);
            var result = await _userManager.AddPasswordAsync(user, request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ResetPasswordResponse { Success = false, Message = errors };
            }

           
            await _userManager.UpdateSecurityStampAsync(user);

            user.ResetToken = null;
            user.ResetTokenExpiresAt = null;
            user.EmailOtpCode = null;
            user.EmailOtpLockedUntil = null;
            user.EmailOtpAttempts = 0;
            user.OtpResendCount = 0;
            user.OtpResendWindowStart = null;

            await _userManager.UpdateAsync(user);

            return new ResetPasswordResponse { Success = true, Message = "Password changed successfully" };
        }
    }
}
