using ExaminationSystem.Common;
using ExaminationSystem.Models;
using ExaminationSystem.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Features.Auth.ForgetPassword.ResetPassword
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Result<string>.Failure("Invalid request.");

           
            var tokenHash = OtpHelper.HashOtp(request.ResetToken);
            if (user.ResetToken != tokenHash)
                return Result<string>.Failure("Token invalid or expired.");

           
            if (user.ResetTokenExpiresAt == null || user.ResetTokenExpiresAt < DateTime.UtcNow)
                return Result<string>.Failure("Token invalid or expired.");

           
            await _userManager.RemovePasswordAsync(user);
            var result = await _userManager.AddPasswordAsync(user, request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<string>.Failure(errors);
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

            return Result<string>.Success("Password changed successfully");
        }
    }
}
