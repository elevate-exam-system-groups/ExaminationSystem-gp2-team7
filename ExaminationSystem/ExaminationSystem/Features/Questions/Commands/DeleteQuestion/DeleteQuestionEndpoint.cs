using ExaminationSystem.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Questions.Commands.DeleteQuestion
{
    [ApiController]
    [Route("api/admin/questions")]
    [Authorize(Roles = "Admin")]
    public class DeleteQuestionEndpoint : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public DeleteQuestionEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

       
        [HttpDelete("{questionId:guid}")]
        public async Task<IActionResult> DeleteQuestion(
            Guid questionId, CancellationToken cancellationToken)
        {
            var command = new DeleteQuestionCommand(questionId);

            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsSuccess)
                return NoContent(); 

            return HandleResult(result);
        }
    }
}
