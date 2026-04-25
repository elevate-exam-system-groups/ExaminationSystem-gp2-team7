using ExaminationSystem.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Diplomas.Commands.UpdateDiploma
{
    [Route("api/diplomas")]
    [Authorize(Roles = "Admin")]
    public class UpdateDiplomaEndpoint(IMediator _mediator) : ApiControllerBase
    {
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateDiploma(Guid id, UpdateDiplomaCommand command)
        {
            var result = await _mediator.Send(command with { Id = id});
            return HandleResult(result);
        }
    }
}
