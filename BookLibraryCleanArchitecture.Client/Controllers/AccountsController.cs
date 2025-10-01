using BookLibraryCleanArchitecture.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookLibraryCleanArchitecture.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }   

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterUserCommand registerRequestDto)
        {
            var result = await _mediator.Send(registerRequestDto);

            return Created(result.UserId.ToString(), result);
        }
    }
}
