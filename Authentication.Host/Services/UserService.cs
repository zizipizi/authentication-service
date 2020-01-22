using System;
using System.Threading.Tasks;
using Authentication.Host.Models;
using Authentication.Host.Repositories;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using NSV.Security.JWT;
using NSV.Security.Password;

namespace Authentication.Host.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;

        public UserService(IUserRepository userRepository, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public async Task<Result<UserResult>> SignOut(TokenModel token)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<UserResult, TokenModel>> ChangePassword(ChangePassModel model)
        {
            throw new NotImplementedException();
        }
    }
}
