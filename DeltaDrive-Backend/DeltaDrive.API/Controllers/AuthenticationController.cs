using DeltaDrive.BL;
using DeltaDrive.BL.Contracts.DTO.Authentication;
using DeltaDrive.BL.Contracts.IService.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DeltaDrive.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IAuthenticationService authService) : ControllerBase
    {
        private readonly IAuthenticationService _authService = authService;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticationRequestDto request)
        {
            var result = await _authService.LoginAsync(request);
            if (result.IsFailed)
            {
                var error = result.Errors.FirstOrDefault();
                if (error is not null && error.Message == FailureCode.InvalidCredentials.Message)
                    return Unauthorized(new { Error = error.Message });

                return BadRequest(new { Error = error?.Message ?? "An error occurred" });
            }
            return Ok(result.Value);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequestDto request)
        {
            var result = await _authService.RegisterAsync(request);
            if (result.IsFailed)
            {
                var error = result.Errors.FirstOrDefault();
                return BadRequest(new { Error = error?.Message ?? "An error occurred" });
            }
            return Ok();
        }
    }
}
