using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Exceptions;
using Authentication.Host.Models;
using Authentication.Host.Repositories;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using NSV.Security.JWT;
using NSV.Security.Password;

namespace Authentication.Host.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;
        private readonly ILogger _logger;
        private readonly IDistributedCache _cache;

        public UserService(IUserRepository userRepository, IPasswordService passwordService, IJwtService jwtService, ILogger<UserService> logger, IDistributedCache cache)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _jwtService = jwtService;
            _logger = logger;
            _cache = cache;
        }

        public async Task<Result<UserResult>> SignOut(BodyTokenModel tokenModel, string id, string accessToken, CancellationToken token)
        {
            try
            {
                //var cacheRefreshToken = await _cache.GetAsync($"blacklist:{}")

                await _userRepository.BlockAllTokensAsync(long.Parse(id), token);

                return new Result<UserResult>(UserResult.Ok);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                return new Result<UserResult>(UserResult.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new Result<UserResult>(UserResult.Error);
            }
        }

        public async Task<Result<UserResult, TokenModel>> ChangePasswordAsync(ChangePassModel model, string id, string accessToken, CancellationToken token)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(long.Parse(id), token);

                var passwordValidateResult = _passwordService.Validate(model.OldPassword, user.Password);

                if (passwordValidateResult.Result == PasswordValidateResult.ValidateResult.Invalid)
                {
                    return new Result<UserResult, TokenModel>(UserResult.WrongPassword, message:"Wrong password");
                }

                var newPasswordHash = _passwordService.Hash(model.NewPassword);

                await _userRepository.UpdateUserPassword(user.Id, newPasswordHash.Hash, token);
                await _userRepository.BlockAllTokensAsync(user.Id, token);

                var access = _jwtService.IssueAccessToken(user.Id.ToString(), user.UserName, user.Role);
                return new Result<UserResult, TokenModel>(UserResult.Ok, model: access.Tokens);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                return new Result<UserResult, TokenModel>(UserResult.Error, message: "DB Error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new Result<UserResult, TokenModel>(UserResult.Error, message:"Service problem");
            }
        }
    }
}
