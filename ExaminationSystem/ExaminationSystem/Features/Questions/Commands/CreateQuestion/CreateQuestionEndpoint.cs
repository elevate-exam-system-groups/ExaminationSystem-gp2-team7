using ExaminationSystem.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Questions.Commands.CreateQuestion
{
    [ApiController]
    [Route("api/admin/quizzes")]
    [Authorize(Roles = "Admin")]
    public class CreateQuestionEndpoint : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public CreateQuestionEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// POST /api/admin/quizzes/{quizId}/questions — إضافة سؤال MCQ جديد
        /// </summary>
        [HttpPost("{quizId:guid}/questions")]
        public async Task<IActionResult> CreateQuestion(
            Guid quizId,
            [FromBody] CreateQuestionRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateQuestionCommand
            {
                QuizId = quizId,
                Text = request.Text,
                Options = request.Options,
                Explanation = request.Explanation
            };

            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsSuccess)
                return StatusCode(StatusCodes.Status201Created, result.Data);

            return HandleResult(result);
        }
    }
}
