using ExaminationSystem.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Students.Queries.GetStudentDashboard
{
    [Route("api/students")]
    [Authorize(Roles = "Student")]
    public class GetStudentDashboardEndpoint(IMediator _mediator) : ApiControllerBase
    {
        [HttpGet("dashboard")]
        public async Task<ActionResult> Dashboard()
        {
            var result = await _mediator.Send(new GetStudentDashboardQuery());
            return HandleResult(result);
        }
    }
}
