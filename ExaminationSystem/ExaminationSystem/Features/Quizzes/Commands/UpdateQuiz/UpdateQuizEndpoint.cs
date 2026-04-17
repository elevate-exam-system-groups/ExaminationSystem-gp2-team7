using ExaminationSystem.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Quizzes.Commands.UpdateQuiz
{
    [ApiController]
    [Route("/api/admin/quizzes")]
    [Authorize(Roles = "Admin")]
    public class UpdateQuizEndpoint(IMediator mediator) : ApiControllerBase
    {
        [HttpPut("{quizId:guid}")]
        public async Task<IActionResult> UpdateQuiz(
            [FromRoute] Guid quizId,
            [FromBody] UpdateQuizRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateQuizCommand(
                this.UserId,
                quizId,
                request.Title,
                request.DurationMinutes,
                request.PassScore,
                request.MaxAttempts,
                request.Instructions
            );

            var result = await mediator.Send(command, cancellationToken);

            return HandleResult(result);
        }
    }
}
