using ExaminationSystem.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Attempts.Queries.GetStudentAttempts
{
    [Route("api/admin/attempts")]
    [Authorize(Roles = "Admin")]
    public class GetStudentAttemptsEndpoint(IMediator _mediator) : ApiControllerBase
    {
        [HttpGet("GetStudentAttempts")]
        public async Task<ActionResult> GetStudentAttempts([FromQuery] GetStudentAttemptsQuery query)
        {
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }
    }
}
