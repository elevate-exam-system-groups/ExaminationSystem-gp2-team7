using ExaminationSystem.Common;
using ExaminationSystem.Models;
using ExaminationSystem.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Features.Auth.ForgetPassword.OTP
{
    public class VerifyOtpHandler : IRequestHandler<VerifyOtpCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public VerifyOtpHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<string>> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            // FluentValidation بيتكفل بالـ validation بتاع Email و OTP

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Result<string>.Failure("Invalid email or OTP.");

            // لو الأكاونت متقفل
            if (user.EmailOtpLockedUntil != null && user.EmailOtpLockedUntil > DateTime.UtcNow)
                return Result<string>.Failure(
                    "Account is locked. Try again later.", StatusCodes.Status403Forbidden);

            // لو الـ OTP انتهى
            if (user.EmailOtpExpiresAt == null || user.EmailOtpExpiresAt < DateTime.UtcNow)
                return Result<string>.Failure("OTP has expired.");

            // لو الـ OTP غلط
            var hashedOtp = OtpHelper.HashOtp(request.Otp);
            if (hashedOtp != user.EmailOtpCode)
            {
                user.EmailOtpAttempts++;
                if (user.EmailOtpAttempts >= 5)
                    user.EmailOtpLockedUntil = DateTime.UtcNow.AddMinutes(15);
                await _userManager.UpdateAsync(user);
                return Result<string>.Failure("Invalid OTP.");
            }

            // OTP صح → اعمل Reset Token
            var resetToken = Guid.NewGuid().ToString();

            user.ResetToken = OtpHelper.HashOtp(resetToken);
            user.ResetTokenExpiresAt = DateTime.UtcNow.AddMinutes(15);

            // نضّف الـ OTP
            user.EmailOtpCode = null;
            user.EmailOtpExpiresAt = null;
            user.EmailOtpAttempts = 0;

            await _userManager.UpdateAsync(user);

            return Result<string>.Success(resetToken);
        }
    }
}
