using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TMS.Controllers.API
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        [Authorize]
        [HttpGet("validate")]
        public IActionResult ValidateToken()
        {
            return Ok(new { success = true });
        }
    }
}
