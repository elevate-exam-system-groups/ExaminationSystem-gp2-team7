using ExaminationSystem.Common;
using ExaminationSystem.Features.Attempts.Queries.ViewResults;
using ExaminationSystem.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Attempts.Queries.GetAttemptResults
{
    [ApiController]
    [Route("api/attempts")]
    [Authorize(Roles = "Student, Admin")]
    public class GetAttemptResultsEndpoint : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public GetAttemptResultsEndpoint(IMediator mediator)
        {
            this._mediator = mediator;
        }
        [HttpGet("{attempId:guid}/results")]
        public async Task<IActionResult> GetResults(
            Guid attemptId, CancellationToken cancellationToken)
        {
            var isAdmin = User.IsInRole("Admin");

            var query = new GetAttemptResultsQuery(attemptId, UserId, isAdmin);

            var result = await _mediator.Send(query, cancellationToken);

            return HandleResult(result);
        }

    }
}
