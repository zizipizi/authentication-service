using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Interfaces;
using Authentication.Data.Models;
using Authentication.Data.Models.Entities;
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
        public async Task BlockUserAsync(int id, CancellationToken token)
        {
            await _userRepository.BlockUserAsync(id, token);
        }

        public async Task CreateUserAsync(UserCreateModel model, CancellationToken token)
        {
            var pass = _passwordService.Hash(model.Password);

            await _userRepository.CreateUserAsync(new UserEntity()
            {
                Login = model.Login,
                Password = pass.Hash,
                Role = model.Role
            }, token);
        }

        public async Task DeleteUserAsync(int id, CancellationToken token)
        {
            await _userRepository.DeleteUserAsync(id, token);
        }
    }

    public interface IUserService
    {
        Task CreateUserAsync(UserCreateModel model, CancellationToken token);

        Task BlockUserAsync(int id, CancellationToken token);

        Task DeleteUserAsync(int id, CancellationToken token);


    }
}
