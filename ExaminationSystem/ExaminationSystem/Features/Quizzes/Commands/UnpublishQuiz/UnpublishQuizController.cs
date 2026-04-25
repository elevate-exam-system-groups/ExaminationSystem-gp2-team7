using ExaminationSystem.Features.PublishQuiz;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Quizzes.Commands.UnpublishQuiz
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnpublishQuizController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UnpublishQuizController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPatch("{id}/unpublish")]
        public async Task<IActionResult> Unpublish(Guid id)
        {
            var result = await _mediator.Send(new PatchUnpublishQuizCommand(id));

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result);

            return Ok(result);
        }


    }
}
