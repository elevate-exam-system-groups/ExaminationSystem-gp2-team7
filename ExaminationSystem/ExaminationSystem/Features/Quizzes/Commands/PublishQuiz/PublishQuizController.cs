using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Quizzes.Commands.PublishQuiz
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishQuizController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PublishQuizController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPatch("{id}/publish")]
        public async Task<IActionResult> Publish(Guid id)
        {
            var result = await _mediator.Send(new PatchPublishQuizCommand(id));

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result);

            return Ok(result);
        }

    }
}
