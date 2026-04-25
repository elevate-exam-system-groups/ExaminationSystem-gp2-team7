using ExaminationSystem.Features.Auth.ForgetPassword.OTP;
using ExaminationSystem.Features.Auth.ForgetPassword.ResetPassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Auth
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(SendOtpCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { message = result });
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { resetToken = result });
        }

        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOtp(ResendOtpCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { message = result });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { message = result });
        }
    }
}
