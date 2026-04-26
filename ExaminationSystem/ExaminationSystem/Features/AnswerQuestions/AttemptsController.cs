using ExaminationSystem.Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Student")]
        [HttpPost("{attemptId}/answer")]
        public async Task<IActionResult> AnswerQuestion(
        Guid attemptId,
        [FromBody] PostAswerQuestionCommand request)
        {
            var userId = User.GetUserId(); 

            var command = new PostAswerQuestionCommand( new AnswerQuestionDTO
            {
                AttemptId = attemptId,
                question_id = request.AnswerQuestionDTO.question_id,
                selected_option_id = request.AnswerQuestionDTO.selected_option_id,
                UserId = Guid.Parse(userId)
            });

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, new { message = result.Error });

            return Ok(result); 
        }



    }
}
