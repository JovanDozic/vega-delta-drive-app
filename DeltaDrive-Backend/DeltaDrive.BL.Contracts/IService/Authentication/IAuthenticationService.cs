using DeltaDrive.BL.Contracts.DTO.Authentication;
using FluentResults;

namespace DeltaDrive.BL.Contracts.IService.Authentication
{
    public interface IAuthenticationService
    {
        Task<Result<AuthenticationResponseDto>> LoginAsync(AuthenticationRequestDto request);
        Task<Result> RegisterAsync(RegisterRequestDto request);
    }
}
