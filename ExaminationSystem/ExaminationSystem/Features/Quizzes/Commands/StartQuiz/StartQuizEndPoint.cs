using ExaminationSystem.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Quizzes.Commands.StartQuiz
{
    [ApiController]
    [Route("api/quizzes")]
    [Authorize(Roles = "Student")]
    public class StartQuizEndPoint(IMediator mediator) : ApiControllerBase
    {
        [HttpPost("{quizId:guid}/start")]

        public async Task<IActionResult> StartQuiz(Guid quizId, CancellationToken cancellationToken)
        {
            var command = new StartQuizCommand(quizId, UserId);

            var result = await mediator.Send(command, cancellationToken);

            return HandleResult(result);
        }
    }
}
