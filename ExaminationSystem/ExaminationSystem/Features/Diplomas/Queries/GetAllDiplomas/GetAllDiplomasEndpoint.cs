using System.Diagnostics.Eventing.Reader;
using ExaminationSystem.Common;
using ExaminationSystem.DbContexts;
using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Diplomas.Queries.GetAllDiplomas
{
    [Route("api/diplomas")]
    [Authorize(Roles = "Student")]
    public class GetAllDiplomasEndpoint(IMediator _mediator) : ApiControllerBase
    {
        [HttpGet("GetAllDiplomas")]
        public async Task<IActionResult> GetAllDiplomas([FromQuery] GetAllDiplomasQuery query)
        {
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }
    }
}