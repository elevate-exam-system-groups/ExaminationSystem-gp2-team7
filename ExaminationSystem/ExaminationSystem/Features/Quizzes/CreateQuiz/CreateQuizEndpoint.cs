using ExaminationSystem.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Quizzes.CreateQuiz
{
    [ApiController]
    [Route("api/admin/quizzes")]
    [Authorize]
    public class CreateQuizEndpoint(IMediator mediator) : ApiControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> CreateQuiz(
            CreateQuizCommand command, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);

            return HandleResult(result);
        }
    }
}
