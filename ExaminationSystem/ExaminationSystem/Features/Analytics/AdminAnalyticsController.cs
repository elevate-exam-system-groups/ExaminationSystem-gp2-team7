using ExaminationSystem.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Analytics
{
    [Route("api/admin/analytics")]
    [Authorize(Roles = "Admin")]
    public class AdminAnalyticsController(IMediator _mediator) : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] GetAdminAnalyticsQuery query)
        {
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }
    }
}
