using System;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Models.Domain;
using Authentication.Data.Repositories;
using Authentication.Host.Enums;
using Authentication.Host.Models;
using NSV.Security.JWT;
using NSV.Security.Password;

namespace Authentication.Host.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService _jwtService;
        private readonly IPasswordService _passwordService;
        private readonly IUserRepository _userRepository;
        public AuthService(IJwtService jwtService, IPasswordService passwordService, IUserRepository userRepository)
        {
            _jwtService = jwtService;
            _passwordService = passwordService;
            _userRepository = userRepository;
        }
        public async Task<Result<AuthResult, TokenModel>> SignIn(LoginModel model, CancellationToken token)
        {
            var user = await _userRepository.GetUserByNameAsync(model.UserName, token);

            if (user != null)
            {
                var validateResult = _passwordService.Validate(model.Password, user.Password);

                if (validateResult.Result == PasswordValidateResult.ValidateResult.Ok)
                {
                    if (user.IsActive)
                    {
                        var access = _jwtService.IssueAccessToken(user.Id.ToString(), user.Login, user.Role.Split());

                        return new Result<AuthResult, TokenModel>(AuthResult.Ok, access.Tokens);
                    }
                    return new Result<AuthResult, TokenModel>(AuthResult.UserBlocked);
                }
            }
            return new Result<AuthResult, TokenModel>(AuthResult.UserNotFound);
        }

        public Task<Result<AuthResult, TokenModel>> RefreshToken(TokenModel model, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }


}
