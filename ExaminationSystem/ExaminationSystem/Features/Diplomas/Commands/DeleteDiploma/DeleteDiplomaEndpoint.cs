using ExaminationSystem.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Diplomas.Commands.DeleteDiploma
{
    [Route("api/diplomas")]
    [Authorize(Roles = "Admin")]
    public class DeleteDiplomaEndpoint(IMediator _mediator) : ApiControllerBase
    {
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiploma(Guid id)
        {
           var result = await _mediator.Send(new DeleteDiplomaCommand(id));
            return HandleResult(result);
        }
    }
}
