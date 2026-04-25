using ExaminationSystem.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Attempts.Queries.GetRemainingTime
{
    [ApiController]
    [Route("api/attempts")]
    [Authorize(Roles = "Student,Admin")]
    public class GetRemainingTimeEndpoint(IMediator mediator) : ApiControllerBase
    {
        [HttpGet("{attemptId:guid}/timer")]
        public async Task<IActionResult> GetRemainingTime(Guid attemptId, CancellationToken cancellationToken)
        {
            var isAdmin = User.IsInRole("Admin");

            var query = new GetRemainingTimeQuery(attemptId, UserId, isAdmin);

            var result = await mediator.Send(query, cancellationToken);

            return HandleResult(result);
        }
    }
}
