using DeltaDrive.BL.Contracts.DTO.Authentication;
using DeltaDrive.BL.Contracts.IService.Authentication;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeltaDrive.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<Result<AuthenticationResponseDto>> Login([FromBody] AuthenticationRequestDto request)
        {
            var result = await _authService.LoginAsync(request);
            if (result.IsFailed)
            {
                return result.ToResult();
            }
            return result;
        }

        [HttpPost("register")]
        public async Task<Result<RegisterResponseDto>> RegisterAsync([FromBody] RegisterRequestDto request)
        {
            return await _authService.RegisterAsync(request);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return Ok("AuthenticationController");
        }
    }
}
