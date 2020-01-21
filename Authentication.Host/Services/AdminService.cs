using System;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data;
using Authentication.Data.Models.Domain;
using Authentication.Data.Repositories;
using Authentication.Host.Models;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using NSV.Security.JWT;
using NSV.Security.Password;

namespace Authentication.Host.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;

        public AdminService(IUserRepository userRepository, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
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
    }
}
