using ExaminationSystem.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExaminationSystem.Features.Attempts.SubmitQuiz
{
    [ApiController]
    [Route("api/attempts")]
    [Authorize]
    public class SubmitQuizEndpoint : ControllerBase
    {
        private readonly IMediator _mediator;

        public SubmitQuizEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{attemptId:guid}/submit")]
        public async Task<IActionResult> Submit(Guid attemptId, CancellationToken cancellationToken)
        {
           
            var studentIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(studentIdClaim, out var studentId))
                return Unauthorized(new { message = "Invalid or missing student identity." });

            
            var command = new SubmitQuizCommand
            {
                AttemptId = attemptId,
                StudentId = studentId
            };

           
            var result = await _mediator.Send(command, cancellationToken);

            
            if (result.IsSuccess)
                return Ok(result.Data);

            return StatusCode(result.StatusCode, new { message = result.Error });
        }
    }
}
