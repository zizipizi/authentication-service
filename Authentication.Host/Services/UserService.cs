using System;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Exceptions;
using Authentication.Host.Models;
using Authentication.Host.Repositories;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using Confluent.Kafka;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using NSV.Security.JWT;
using NSV.Security.Password;
using Processing.Kafka.Producer;

namespace Authentication.Host.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;
        private readonly ILogger _logger;
        private readonly IProducerFactory<int, string> _kafka;

        public UserService(IUserRepository userRepository, 
            ITokenRepository tokenRepository, 
            IPasswordService passwordService, 
            IJwtService jwtService, 
            ILogger<UserService> logger,
            IProducerFactory<int,string> kafka
            )
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _passwordService = passwordService;
            _jwtService = jwtService;
            _logger = logger;
            _kafka = kafka;
        }

        public async Task<Result<UserResult>> SignOutAsync(long id, string refreshJti, CancellationToken cancellationToken)
        {
            try
            {
                await _tokenRepository.BlockRefreshTokenAsync(refreshJti, cancellationToken);
                using (var mes = _kafka.Create("BlockedTokens"))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        var res = await mes.SendAsync(1, refreshJti);
                        _logger.LogInformation("Sended");
                    }
                }
                return new Result<UserResult>(UserResult.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new Result<UserResult>(UserResult.Error);
            }
        }

        public async Task<Result<UserResult, TokenModel>> ChangePasswordAsync(ChangePassModel model, long id, string accessToken, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id, cancellationToken);

                var passwordValidateResult = _passwordService.Validate(model.OldPassword, user.Password);

                if (passwordValidateResult.Result == PasswordValidateResult.ValidateResult.Invalid)
                {
                    return new Result<UserResult, TokenModel>(UserResult.WrongPassword, message: "Wrong password");
                }

                var newPasswordHash = _passwordService.Hash(model.NewPassword);

                await _userRepository.UpdateUserPassword(user.Id, newPasswordHash.Hash, cancellationToken);
                await _tokenRepository.BlockAllTokensAsync(user.Id, cancellationToken);

                var access = _jwtService.IssueAccessToken(user.Id.ToString(), user.Login, user.Role);
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
