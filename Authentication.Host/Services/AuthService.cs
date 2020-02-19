﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Exceptions;
using Authentication.Host.Models;
using Authentication.Host.Repositories;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSV.Security.JWT;
using NSV.Security.Password;

namespace Authentication.Host.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService _jwtService;
        private readonly IPasswordService _passwordService;
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly ILogger _logger;
        private readonly IDistributedCache _cache;

        public AuthService(IJwtService jwtService, 
            IPasswordService passwordService, 
            IUserRepository userRepository, 
            ITokenRepository tokenRepository,
            ILogger<AuthService> logger, 
            IDistributedCache cache)
        {
            _jwtService = jwtService;
            _passwordService = passwordService;
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _logger = logger;
            _cache = cache;
        }

        public async Task<Result<AuthResult, TokenModel>> SignIn(LoginModel model, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetUserByNameAsync(model.UserName, cancellationToken);

                var validateResult = _passwordService.Validate(model.Password, user.Password);

                if (validateResult.Result != PasswordValidateResult.ValidateResult.Ok)
                {
                    _logger.LogError($"Password Validation result while signin {validateResult.Result.ToString()}");
                    return new Result<AuthResult, TokenModel>(AuthResult.WrongLoginOrPass, message: "Wrong login or password");
                }

                if (!user.IsActive)
                    return new Result<AuthResult, TokenModel>(AuthResult.UserBlocked, message: "User is blocked");


                var access = _jwtService.IssueAccessToken(user.Id.ToString(), user.Login, user.Role);

                await _tokenRepository.AddTokensAsync(user.Id, access.Tokens, cancellationToken);

                return new Result<AuthResult, TokenModel>(AuthResult.Ok, access.Tokens);

            }
            catch (EntityNotFoundException)
            {
                return new Result<AuthResult, TokenModel>(AuthResult.UserNotFound, message: "User not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new Result<AuthResult, TokenModel>(AuthResult.Error, message: "DB Error");
            }
        }

        public async Task<Result<AuthResult, TokenModel>> RefreshToken(BodyTokenModel model, CancellationToken cancellationToken)
        {
            try
            {
                var validateResult = _jwtService.RefreshAccessToken(model.AccessToken, model.RefreshToken);

                if (validateResult.Result != JwtTokenResult.TokenResult.Ok)
                {
                    _logger.LogError($"Validation result while refreshing: {validateResult.Result.ToString()}");
                    return new Result<AuthResult, TokenModel>(AuthResult.TokenValidationProblem, message: "Validation problem, please try again");
                }

                var isTokenBlocked = await _tokenRepository.IsRefreshTokenBlockedAsync(validateResult.Tokens.RefreshToken.Jti, cancellationToken);

                if (isTokenBlocked)
                    return new Result<AuthResult, TokenModel>(AuthResult.TokenIsBlocked, message: "Token is blocked");

                await _tokenRepository.AddTokensAsync(long.Parse(validateResult.UserId), validateResult.Tokens, cancellationToken);
                return new Result<AuthResult, TokenModel>(AuthResult.Ok, validateResult.Tokens);
            }
            catch (EntityNotFoundException ex)
            {
                return new Result<AuthResult, TokenModel>(AuthResult.Error, message:ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new Result<AuthResult, TokenModel>(AuthResult.Error, message:"DB Error");
            }
        }
    }
}
