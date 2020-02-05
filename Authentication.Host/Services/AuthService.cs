using System;
using System.Linq;
using System.Text;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data;
using Authentication.Data.Exceptions;
using Authentication.Data.Models.Domain;
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
        private readonly ILogger _logger;
        private readonly IDistributedCache _cache;

        public AuthService(IJwtService jwtService, IPasswordService passwordService, IUserRepository userRepository, ILogger<AuthService> logger, IDistributedCache cache)
        {
            _jwtService = jwtService;
            _passwordService = passwordService;
            _userRepository = userRepository;
            _logger = logger;
            _cache = cache;
        }

        public async Task<Result<AuthResult, TokenModel>> SignIn(LoginModel model, CancellationToken token)
        {
            try
            {
                var user = await _userRepository.GetUserByNameAsync(model.UserName, token);

                var validateResult = _passwordService.Validate(model.Password, user.Password);

                if (validateResult.Result == PasswordValidateResult.ValidateResult.Ok)
                {
                    if (user.IsActive)
                    {
                        var access = _jwtService.IssueAccessToken(user.Id.ToString(), user.Login, user.Role);
                        await _userRepository.AddTokensAsync(access, token);
                        return new Result<AuthResult, TokenModel>(AuthResult.Ok, access.Tokens);
                    }
                    return new Result<AuthResult, TokenModel>(AuthResult.UserBlocked, message: "User is blocked");
                }
                return new Result<AuthResult, TokenModel>(AuthResult.WrongLoginOrPass, message: "Wrong login or password");
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

        public async Task<Result<AuthResult, TokenModel>> RefreshToken(BodyTokenModel model, CancellationToken token)
        {

            var tok = new BodyTokenModel
            {
                AccessToken = "asdads23123",
                RefreshToken = "joijojoijwejr823"
            };

            _cache.SetString("tokModel", JsonConvert.SerializeObject(tok));

            var myModel = JsonConvert.DeserializeObject<BodyTokenModel>(_cache.GetString("tokModel"));

            var s = Encoding.UTF8.GetString(_cache.Get("MyK2ey"));

            try
            {
                var validateResult = _jwtService.RefreshAccessToken(model.AccessToken, model.RefreshToken);

                await _userRepository.CheckRefreshTokenAsync(validateResult, token);

                if (validateResult.Result == JwtTokenResult.TokenResult.Ok)
                {
                    await _userRepository.AddTokensAsync(validateResult, token);
                    return new Result<AuthResult, TokenModel>(AuthResult.Ok, validateResult.Tokens);
                }

                return new Result<AuthResult, TokenModel>(AuthResult.TokenValidationProblem);
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
