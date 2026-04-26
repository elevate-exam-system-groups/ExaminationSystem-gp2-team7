using ExaminationSystem.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Diplomas.Commands.AddDiploma
{
    [Route("api/diplomas")]
    [Authorize(Roles = "Admin")]
    public class AddDiplomaEndpoint(IMediator _mediator) : ApiControllerBase
    {
        [HttpPost("AddDiploma")]
        public async Task<ActionResult> AddDiploma(AddDiplomaCommand command)
        {
            var result =await _mediator.Send(command);
            return HandleResult(result);
        }
    }
}
