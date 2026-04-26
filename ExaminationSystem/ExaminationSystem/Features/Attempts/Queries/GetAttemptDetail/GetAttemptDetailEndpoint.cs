using ExaminationSystem.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Attempts.Queries.GetAttemptDetail
{
    [ApiController]
    [Route("api/student/attempts")]
    [Authorize(Roles = "Student")]
    public class GetAttemptDetailEndpoint : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public GetAttemptDetailEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

      
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
