using MediatR;

using ExaminationSystem.Contracts;
using ExaminationSystem.Models;
using ExaminationSystem.Services;

using Microsoft.AspNetCore.Identity;
namespace ExaminationSystem.Features.Auth.ForgetPassword.OTP
{
    public class SendOtpHandler: IRequestHandler<SendOtpCommand, string>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        public SendOtpHandler(
            UserManager<ApplicationUser> userManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }
        public async Task<string> Handle(SendOtpCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Email))
                throw new Exception("Email is required");
            if (!request.Email.Contains("@"))
                throw new Exception("Invalid email format");
            
            var user = await _userManager.FindByEmailAsync(request.Email);
           
            if (user == null)
                return "Verification code has been sent to your email";
            
            var otp = OtpHelper.GenerateOtp();
            
            user.EmailOtpCode = OtpHelper.HashOtp(otp);
            user.EmailOtpExpiresAt = DateTime.UtcNow.AddMinutes(10);
            user.EmailOtpAttempts = 0;
           
            await _userManager.UpdateAsync(user);
           
            await _emailService.SendEmailAsync(
                user.Email,
                "Password Reset Code",
                $"<h2>Your verification code: {otp}</h2><p>Valid for 10 minutes</p>"
            );
            return "Verification code has been sent to your email";
        }

    }
}
