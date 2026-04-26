using ExaminationSystem.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Attempts.Queries.GetAttemptDetail
{
    [ApiController]
    [Route("api/attempts")]
    [Authorize(Roles = "Student, Admin")]
    public class GetAttemptDetailEndpoint(IMediator _mediator) : ApiControllerBase
    { 
        [HttpGet("{attemptId:guid}")]
        public async Task<IActionResult> GetAttemptDetail(
            Guid attemptId, CancellationToken cancellationToken)
        {
            var query = new GetAttemptDetailQuery
            {
                AttemptId = attemptId,
                StudentId = UserId
            };

            var result = await _mediator.Send(query, cancellationToken);

            return HandleResult(result);
        }
    }
}
