﻿using AutoMapper;
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
            if (user is null)
            {
                return Result.Fail(FailureCode.UserNotFound);
            }
            else if (!user.VerifyPassword(request.Password))
            {
                return Result.Fail(FailureCode.InvalidCredentials);
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

            if (await _unitOfWork.UserRepo().ExistByEmail(user.Email))
            {
                return Result.Fail(FailureCode.NonUniqueEmail);
            }
            if (!user.Validate())
            {
                return Result.Fail(FailureCode.InvalidArgument);
            }

            user.SecurePassword();
            await _unitOfWork.UserRepo().AddAsync(user);
            await _unitOfWork.SaveAsync();

            return Result.Ok();
        }
    }
}
