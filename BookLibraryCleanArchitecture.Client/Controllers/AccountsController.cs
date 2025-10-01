using BookLibraryCleanArchitecture.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BookLibraryCleanArchitecture.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterRequestDto registerRequestDto)
        {
            // The Ok() method returns an OkObjectResult, which is not awaitable.
            // Simply return Ok() without await.
            return Ok("Register endpoint");
        }
    }
}
