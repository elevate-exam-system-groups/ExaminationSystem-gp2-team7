using ExaminationSystem.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Attempts.Queries.GetQuizHistory
{
    [ApiController]
    [Route("api/student/attempts")]
    [Authorize(Roles = "Student")]
    public class GetQuizHistoryEndpoint : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public GetQuizHistoryEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// GET /api/student/attempts — لسته المحاولات بتاعت الطالب (paginated + filters)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetQuizHistory(
            [FromQuery(Name = "quiz_id")] Guid? quizId,
            [FromQuery(Name = "diploma_id")] Guid? diplomaId,
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "per_page")] int perPage = 10,
            CancellationToken cancellationToken = default)
        {
            var query = new GetQuizHistoryQuery
            {
                StudentId = UserId,
                QuizId = quizId,
                DiplomaId = diplomaId,
                Page = page,
                PerPage = perPage
            };

            var result = await _mediator.Send(query, cancellationToken);

            return HandleResult(result);
        }
    }
}
