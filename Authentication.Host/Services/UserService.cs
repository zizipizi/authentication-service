using System;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Models.Domain;
using Authentication.Data.Repositories;
using Authentication.Host.Models;
using NSV.Security.JWT;
using NSV.Security.Password;

namespace Authentication.Host.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;

        public UserService(IUserRepository userRepository, IPasswordService passwordService, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }
        public async Task<UserServiceResult> BlockUserAsync(int id, CancellationToken token)
        {
            try
            {
                await _userRepository.BlockUserAsync(id, token);
                return UserServiceResult.Ok();
            }
            catch (Exception)
            {
                return UserServiceResult.UserNotFound();
            }
        }

        public async Task<UserServiceResult> CreateUserAsync(UserCreateModel model, CancellationToken token)
        {
            try
            {
                var pass = _passwordService.Hash(model.Password);

                await _userRepository.CreateUserAsync(new User()
                {
                    Login = model.Login,
                    Password = pass.Hash,
                    Role = model.Role
                }, token);

                return UserServiceResult.Ok();
            }
            catch (Exception)
            {
                return UserServiceResult.UserExist();
            }
        }

        public async Task<UserServiceResult> DeleteUserAsync(int id, CancellationToken token)
        {
            try
            {
                await _userRepository.DeleteUserAsync(id, token);
                return UserServiceResult.Ok();
            }
            catch (Exception)
            {
                return UserServiceResult.UserNotFound();
            }
        }

        public async Task<UserServiceResult> SignIn(LoginModel model)
        {
            var user = await _userRepository.GetUserByNameAsync(model.UserName, CancellationToken.None);

            if (user != null)
            {
                var validateResult = _passwordService.Validate(model.Password, user.Password);

                if (validateResult.Result == PasswordValidateResult.ValidateResult.Ok)
                {
                    if (user.IsActive)
                    {
                        var access = _jwtService.IssueAccessToken(user.Id.ToString(), user.Login, user.Role.Split());

                        return UserServiceResult.Ok(access.Tokens, user);
                    }
                    return UserServiceResult.Blocked();
                }
            }
            return UserServiceResult.UserNotFound();
        }
    }
}
