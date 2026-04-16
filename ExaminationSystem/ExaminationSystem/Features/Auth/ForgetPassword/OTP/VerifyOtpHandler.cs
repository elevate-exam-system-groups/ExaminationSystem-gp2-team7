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
           
            if (string.IsNullOrEmpty(request.Email))
                throw new Exception("Email is required");
            if (string.IsNullOrEmpty(request.Otp) || request.Otp.Length != 6)
                throw new Exception("OTP must be 6 digits");

            
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new Exception("Invalid email or OTP");

           
            if (user.EmailOtpLockedUntil != null && user.EmailOtpLockedUntil > DateTime.UtcNow)
                throw new Exception("Account is locked. Try again later");

          
            if (user.EmailOtpExpiresAt == null || user.EmailOtpExpiresAt < DateTime.UtcNow)
                throw new Exception("OTP expired");

         
            var hashedOtp = OtpHelper.HashOtp(request.Otp);
            if (hashedOtp != user.EmailOtpCode)
            {
                user.EmailOtpAttempts++;
                if (user.EmailOtpAttempts >= 5)
                    user.EmailOtpLockedUntil = DateTime.UtcNow.AddMinutes(15);
                await _userManager.UpdateAsync(user);
                throw new Exception("Invalid OTP");
            }

          
            var resetToken = Guid.NewGuid().ToString();

           
            user.ResetToken = OtpHelper.HashOtp(resetToken);
            user.ResetTokenExpiresAt = DateTime.UtcNow.AddMinutes(15);

           
            user.EmailOtpCode = null;
            user.EmailOtpExpiresAt = null;
            user.EmailOtpAttempts = 0;

            await _userManager.UpdateAsync(user);

           
            return resetToken;
        }
    }
}
