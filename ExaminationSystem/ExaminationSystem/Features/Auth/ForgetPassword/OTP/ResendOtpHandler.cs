using ExaminationSystem.Contracts;
using ExaminationSystem.Models;
using ExaminationSystem.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Features.Auth.ForgetPassword.OTP
{
    public class ResendOtpHandler: IRequestHandler<ResendOtpCommand, string>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        public ResendOtpHandler(
            UserManager<ApplicationUser> userManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }
        public async Task<string> Handle(ResendOtpCommand request, CancellationToken cancellationToken)
        {
           
            if (string.IsNullOrEmpty(request.Email))
                throw new Exception("Email is required");
          
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return "Verification code has been sent to your email";
           
            if (user.OtpResendWindowStart != null)
            {
               
                if (user.OtpResendWindowStart > DateTime.UtcNow.AddHours(-1))
                {
                    if (user.OtpResendCount >= 3)
                        throw new Exception("Resend limit reached. Try again after 1 hour");
                }
                else
                {
                   
                    user.OtpResendCount = 0;
                    user.OtpResendWindowStart = DateTime.UtcNow;
                }
            }
            else
            {
              
                user.OtpResendWindowStart = DateTime.UtcNow;
            }
           
            user.EmailOtpCode = null;
           
            var otp = OtpHelper.GenerateOtp();
         
            user.EmailOtpCode = OtpHelper.HashOtp(otp);
            user.EmailOtpExpiresAt = DateTime.UtcNow.AddMinutes(10);
            user.EmailOtpAttempts = 0;
            user.OtpResendCount++;
          
            await _userManager.UpdateAsync(user);
           
            await _emailService.SendEmailAsync(
                user.Email,
                "Password Reset Code",
                $"<h2>Your new verification code: {otp}</h2><p>Valid for 10 minutes</p>"
            );
            return "Verification code has been sent to your email";
        }
    }
}
