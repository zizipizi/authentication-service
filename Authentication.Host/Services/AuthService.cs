using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Models;
using Authentication.Host.Models.Translators;
using Authentication.Host.Repositories;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSV.Security.JWT;
using NSV.Security.Password;

namespace Authentication.Host.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService _jwtService;
        private readonly IPasswordService _passwordService;
        private readonly ILogger _logger;
        private readonly ICacheRepository _cacheRepository;
        private readonly IAuthRepository _authRepository;

        public AuthService(IJwtService jwtService, 
            IPasswordService passwordService,
            ICacheRepository cacheRepository,
            IAuthRepository authRepository,
            ILogger<AuthService> logger = null)
        {
            _jwtService = jwtService;
            _passwordService = passwordService;
            _cacheRepository = cacheRepository;
            _authRepository = authRepository;
            _logger = logger ?? new NullLogger<AuthService>();
        }

        public async Task<Result<HttpStatusCode, BodyTokenModel>> SignIn(LoginModel model, CancellationToken cancellationToken)
        {
            var userResult = await _authRepository.GetUserByNameAsync(model.UserName, cancellationToken);
            
            switch (userResult.Value)
            {
                case AuthRepositoryResult.UserNotFound:
                    return new Result<HttpStatusCode, BodyTokenModel>(HttpStatusCode.NotFound);
                case AuthRepositoryResult.Error:
                    return new Result<HttpStatusCode, BodyTokenModel>(HttpStatusCode.ServiceUnavailable, message: "Please try again");
            }

            var validateResult = _passwordService.Validate(model.Password, userResult.Model.Password);

            if (validateResult.Result != PasswordValidateResult.ValidateResult.Ok)
            {
                _logger.LogError($"Password Validation result while signin {validateResult.Result.ToString()}");
                return new Result<HttpStatusCode, BodyTokenModel>(HttpStatusCode.Unauthorized, message: "Wrong login or password");
            }

            if (!userResult.Model.IsActive)
                return new Result<HttpStatusCode, BodyTokenModel>(HttpStatusCode.Unauthorized, message: "User is blocked");

            var newTokens = _jwtService.IssueAccessToken(userResult.Model.Id.ToString(), userResult.Model.Login, userResult.Model.Role);
            
            await _authRepository.AddTokensAsync(userResult.Model.Id, newTokens.Tokens, model.IpAddress, cancellationToken);

            return new Result<HttpStatusCode, BodyTokenModel>(HttpStatusCode.OK, newTokens.Tokens.toBodyTokenModel());
        }

        public async Task<Result<HttpStatusCode, BodyTokenModel>> RefreshToken(BodyTokenModel model, CancellationToken cancellationToken)
        {
            var validateResult = _jwtService.RefreshAccessToken(model.AccessToken, model.RefreshToken);

            if (validateResult.Result == JwtTokenResult.TokenResult.Ok)
            {
                var isTokenBlocked = await _cacheRepository.IsRefreshTokenBlockedAsync(validateResult.Tokens.RefreshToken.Jti, cancellationToken);

                if (isTokenBlocked.Value == CacheRepositoryResult.IsBlocked)
                    return new Result<HttpStatusCode, BodyTokenModel>(HttpStatusCode.Unauthorized, message:"Token is blocked");

                var addToken = await _authRepository.AddTokensAsync(long.Parse(validateResult.UserId), validateResult.Tokens, cancellationToken: cancellationToken);

                if (addToken.Value != AuthRepositoryResult.Ok)
                    return new Result<HttpStatusCode, BodyTokenModel>(HttpStatusCode.ServiceUnavailable, message: "Please, try again");

                return new Result<HttpStatusCode, BodyTokenModel>(HttpStatusCode.OK, validateResult.Tokens.toBodyTokenModel());
            }

            return new Result<HttpStatusCode, BodyTokenModel>(HttpStatusCode.Unauthorized, message: validateResult.Result.ToString());
        }
    }
}
