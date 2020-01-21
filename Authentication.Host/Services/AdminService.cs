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
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;

        public AdminService(IUserRepository userRepository, IPasswordService passwordService, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }

        public async Task<Result<AdminResult>> BlockUserAsync(int id, CancellationToken token)
        {
            try
            {
                await _userRepository.BlockUserAsync(id, token);
                return new Result<AdminResult>(AdminResult.Ok);
            }
            catch (Exception)
            {
                return new Result<AdminResult>(AdminResult.UserNotFound);
            }
        }

        public async Task<Result<AdminResult>> CreateUserAsync(UserCreateModel model, CancellationToken token)
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

                return new Result<AdminResult>(AdminResult.Ok);
            }
            catch (Exception)
            {
                return new Result<AdminResult>(AdminResult.UserExist);
            }
        }

        public async Task<Result<AdminResult>> DeleteUserAsync(int id, CancellationToken token)
        {
            try
            {
                await _userRepository.DeleteUserAsync(id, token);
                return new Result<AdminResult>(AdminResult.Ok);
            }
            catch (Exception)
            {
                return new Result<AdminResult>(AdminResult.UserNotFound);
            }
        }

        ////TODO Запись токенов в базу
        //public async Task<AdminServiceResult> SignIn(LoginModel model, CancellationToken token)
        //{
        //    var user = await _userRepository.GetUserByNameAsync(model.UserName, token);

        //    if (user != null)
        //    {
        //        var validateResult = _passwordService.Validate(model.Password, user.Password);

        //        if (validateResult.Result == PasswordValidateResult.ValidateResult.Ok)
        //        {
        //            if (user.IsActive)
        //            {
        //                var access = _jwtService.IssueAccessToken(user.Id.ToString(), user.Login, user.Role.Split());

        //                //return AdminServiceResult.Ok(access.Tokens, user);
        //            }
        //            return AdminServiceResult.UserBlocked();
        //        }
        //    }
        //    return AdminServiceResult.UserNotFound();
        //}

        //public async Task<AdminServiceResult> RefreshTokenAsync(TokenModel model, CancellationToken token)
        //{
        //    var validateResult = _jwtService.RefreshAccessToken(model.AccessToken.Value, model.RefreshToken.Value);

        //    if (validateResult.Result != JwtTokenResult.TokenResult.Ok)
        //    {
        //        //return AdminServiceResult.TokenNotValidate();
        //    }
        //    return AdminServiceResult.Ok();
        //}
    }
}
