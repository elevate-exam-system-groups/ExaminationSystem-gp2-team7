using ExaminationSystem.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Questions.Commands.UpdateQuestion
{
    [ApiController]
    [Route("api/admin/questions")]
    [Authorize(Roles = "Admin")]
    public class UpdateQuestionEndpoint : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public UpdateQuestionEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

       
        [HttpPut("{questionId:guid}")]
        public async Task<IActionResult> UpdateQuestion(
            Guid questionId,
            [FromBody] UpdateQuestionRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateQuestionCommand
            {
                QuestionId = questionId,
                Text = request.Text,
                Options = request.Options,
                Explanation = request.Explanation,
                CorrectAnswer = request.CorrectAnswer
            };

            var result = await _mediator.Send(command, cancellationToken);

            return HandleResult(result);
        }
    }
}
