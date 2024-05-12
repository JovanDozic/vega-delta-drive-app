using DeltaDrive.BL;
using DeltaDrive.BL.Contracts.DTO.Authentication;
using DeltaDrive.BL.Contracts.IService.Authentication;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace DeltaDrive.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IAuthenticationService authService) : ControllerBase
    {
        private readonly IAuthenticationService _authService = authService;

        [HttpPost("login")]
        public async Task<Result<AuthenticationResponseDto>> Login([FromBody] AuthenticationRequestDto request)
        {
            // TODO: Fix result handling!!!!!!!!

            var result = await _authService.LoginAsync(request);
            if (result.IsFailed)
            {
                return Result.Fail(FailureCode.InvalidCredentials);
            }
            return result;
        }

        [HttpPost("register")]
        public async Task<Result> RegisterAsync([FromBody] RegisterRequestDto request)
        {
            return await _authService.RegisterAsync(request);
        }
    }
}
