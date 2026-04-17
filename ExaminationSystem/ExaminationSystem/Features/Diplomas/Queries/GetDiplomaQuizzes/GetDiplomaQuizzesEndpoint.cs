using ExaminationSystem.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Diplomas.Queries.GetDiplomaQuizzes
{
    [ApiController]
    [Route("api/diplomas")]
    [Authorize(Roles = "Student")]
    public class GetDiplomaQuizzesEndpoint(IMediator mediator) : ApiControllerBase
    {

        [HttpGet("{diplomaId:guid}/quizzes")]
        public async Task<IActionResult> GetQuizzes(Guid diplomaId, CancellationToken cancellationToken)
        {
            var query = new GetDiplomaQuizzesQuery(diplomaId, UserId);

            var result = await mediator.Send(query, cancellationToken);

            return HandleResult(result);
        }

    }
}
