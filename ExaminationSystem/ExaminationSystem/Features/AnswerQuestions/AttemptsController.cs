using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.AnswerQuestions
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttemptsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AttemptsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        //[HttpPost("api/attempts/{attemptId}/answer")]
        //public async Task<IActionResult> AnswerQuestion( int attemptId, [FromBody] AnswerQuestionDTO dto)
        //{
        //    var userId = User.GetUserId(); // حسب implementation عندك

        //    var command = new PostAswerQuestionCommand
        //    {
        //        AttemptId = attemptId,
        //        UserId = userId,
        //        AnswerQuestionDTO = dto
        //    };

        //    var result = await mediator.Send(command);

        //    return StatusCode(result.StatusCode, result);
        //}
    }
}
