using ExaminationSystem.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExaminationSystem.Features.Diplomas.GetDiplomaQuizzes
{
    [ApiController]
    [Route("api/diplomas")]
    [Authorize]
    public class GetDiplomaQuizzesEndpoint(IMediator mediator) : ApiControllerBase
    {

        [HttpGet("{diplomaId:guid}/quizzes")]
        public async Task<IActionResult> GetQuizzes(Guid diplomaId, CancellationToken cancellationToken)
        {
            if (StudentId == Guid.Empty)
                return Unauthorized();

            var query = new GetDiplomaQuizzesQuery(diplomaId, StudentId);

            var result = await mediator.Send(query, cancellationToken);

            return HandleResult(result);
        }

    }
}
