using Authentication.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authentication.Host.Enums;
using Authentication.Host.Models;
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

        public async Task<Result<UserResult>> SignOut()
        {
            throw new NotImplementedException();
        }

        public async Task<Result<UserResult>> ChangePassword(ChangePassModel model)
        {
            throw new NotImplementedException();
        }
    }
}
