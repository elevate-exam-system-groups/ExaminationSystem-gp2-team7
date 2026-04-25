using ExaminationSystem.Common;
using ExaminationSystem.Features.Questions.Commands.CreateQuestion;
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

        /// <summary>
        /// PUT /api/admin/questions/{questionId} — تعديل سؤال MCQ
        /// </summary>
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
                Explanation = request.Explanation
            };

            var result = await _mediator.Send(command, cancellationToken);

            return HandleResult(result);
        }
    }
}
