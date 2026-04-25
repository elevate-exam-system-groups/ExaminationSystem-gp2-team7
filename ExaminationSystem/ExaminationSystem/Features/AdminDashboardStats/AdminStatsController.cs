using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.AdminDashboardStats
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminStatsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminStatsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("api/admin/stats")]
        public async Task<IActionResult> GetStats()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            var result = await _mediator.Send(new GetAdminStatsQuery
            {
                UserRole = role!
            });

            return StatusCode(result.StatusCode, result);
        }

    }
}
