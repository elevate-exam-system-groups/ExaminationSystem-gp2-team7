using ExaminationSystem.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Quizzes.Commands.CreateQuiz
{
    [ApiController]
    [Route("api/admin/quizzes")]
    [Authorize(Roles = "Administrator")]
    public class CreateQuizEndpoint(IMediator mediator) : ApiControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> CreateQuiz(
            CreateQuizRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateQuizCommand(
                UserId,
                request.Title,
                request.DiplomaId,
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
