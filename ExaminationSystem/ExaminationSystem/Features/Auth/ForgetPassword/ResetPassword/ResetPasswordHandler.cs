using ExaminationSystem.Common.Exceptions;
using ExaminationSystem.Models;
using ExaminationSystem.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Features.Auth.ForgetPassword.ResetPassword
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, string>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            // FluentValidation بيتكفل بالـ validation

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new BadRequestException("Invalid request.");

            // شيك الـ Token
            var tokenHash = OtpHelper.HashOtp(request.ResetToken);
            if (user.ResetToken != tokenHash)
                throw new BadRequestException("Token invalid or expired.");

            // شيك صلاحية الـ Token
            if (user.ResetTokenExpiresAt == null || user.ResetTokenExpiresAt < DateTime.UtcNow)
                throw new BadRequestException("Token invalid or expired.");

            // غيّر الباسورد
            await _userManager.RemovePasswordAsync(user);
            var result = await _userManager.AddPasswordAsync(user, request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BadRequestException(errors);
            }

            // حدّث الـ Security Stamp ونضّف الـ Tokens
            await _userManager.UpdateSecurityStampAsync(user);

            user.ResetToken = null;
            user.ResetTokenExpiresAt = null;
            user.EmailOtpCode = null;
            user.EmailOtpLockedUntil = null;
            user.EmailOtpAttempts = 0;
            user.OtpResendCount = 0;
            user.OtpResendWindowStart = null;

            await _userManager.UpdateAsync(user);

            return "Password changed successfully";
        }
    }
}
