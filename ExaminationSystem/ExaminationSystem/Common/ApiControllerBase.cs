using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExaminationSystem.Common
{
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        protected Guid StudentId => Guid.TryParse(
            User.FindFirstValue(ClaimTypes.NameIdentifier), out var id)?
            id : Guid.Empty;

        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
                return Ok(result.Data);

            return StatusCode(result.StatusCode, new ProblemDetails
            {
                Status = result.StatusCode,
                Detail = result.Error,
                Instance = HttpContext.Request.Path
            });
        }
    }
}
