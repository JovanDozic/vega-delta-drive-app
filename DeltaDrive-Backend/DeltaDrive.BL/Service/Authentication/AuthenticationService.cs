using AutoMapper;
using DeltaDrive.BL.Contracts.DTO.Authentication;
using DeltaDrive.BL.Contracts.IService.Authentication;
using DeltaDrive.DA.Contracts;
using DeltaDrive.DA.Contracts.Model;
using FluentResults;

namespace DeltaDrive.BL.Service.Authentication
{
    public class AuthenticationService(IUnitOfWork unitOfWork, IMapper mapper) : BaseService(unitOfWork, mapper), IAuthenticationService
    {
        public async Task<Result<AuthenticationResponseDto>> LoginAsync(AuthenticationRequestDto request)
        {
            var user = await _unitOfWork.UserRepo().GetByEmailAsync(request.Email);
            if (user is null || !user.VerifyPassword(request.Password))
            {
                return Result.Fail(FailureCode.Forbidden);
            }
            var token = _unitOfWork.TokenGeneratorRepo().GenerateAccessToken(user);
            return Result.Ok(new AuthenticationResponseDto()
            {
                Id = user.Id,
                AccessToken = token
            });
        }

        public async Task<Result> RegisterAsync(RegisterRequestDto request)
        {
            User user = new()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Birthday = request.Birthday,
                Password = request.Password
            };
            user.SecurePassword();

            // TODO: Data validation goes here (unique email, password complexity, etc.)

            await _unitOfWork.UserRepo().AddAsync(user);
            await _unitOfWork.SaveAsync();

            return Result.Ok();
        }
    }
}
