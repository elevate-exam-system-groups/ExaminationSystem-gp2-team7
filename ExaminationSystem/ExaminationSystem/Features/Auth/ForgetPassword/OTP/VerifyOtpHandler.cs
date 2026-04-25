using ExaminationSystem.Common.Exceptions;
using ExaminationSystem.Models;
using ExaminationSystem.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Features.Auth.ForgetPassword.OTP
{
    public class VerifyOtpHandler : IRequestHandler<VerifyOtpCommand, string>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public VerifyOtpHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            // FluentValidation بيتكفل بالـ validation بتاع Email و OTP

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new BadRequestException("Invalid email or OTP.");

            // لو الأكاونت متقفل
            if (user.EmailOtpLockedUntil != null && user.EmailOtpLockedUntil > DateTime.UtcNow)
                throw new ForbiddenException("Account is locked. Try again later.");

            // لو الـ OTP انتهى
            if (user.EmailOtpExpiresAt == null || user.EmailOtpExpiresAt < DateTime.UtcNow)
                throw new BadRequestException("OTP has expired.");

            // لو الـ OTP غلط
            var hashedOtp = OtpHelper.HashOtp(request.Otp);
            if (hashedOtp != user.EmailOtpCode)
            {
                user.EmailOtpAttempts++;
                if (user.EmailOtpAttempts >= 5)
                    user.EmailOtpLockedUntil = DateTime.UtcNow.AddMinutes(15);
                await _userManager.UpdateAsync(user);
                throw new BadRequestException("Invalid OTP.");
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

            return resetToken;
        }
    }
}
