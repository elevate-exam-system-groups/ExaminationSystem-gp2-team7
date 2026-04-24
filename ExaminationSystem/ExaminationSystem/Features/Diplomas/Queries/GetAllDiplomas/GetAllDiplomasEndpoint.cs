using System.Diagnostics.Eventing.Reader;
using ExaminationSystem.DbContexts;
using ExaminationSystem.Models;
using ExaminationSystem.Models.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Diplomas.Queries.GetAllDiplomas
{
    [ApiController]
    [Route("api/diplomas")]
    [Authorize(Roles = "User")]
    public class GetAllDiplomasEndpoint(IMediator _mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllDiplomas([FromQuery] GetAllDiplomasQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}