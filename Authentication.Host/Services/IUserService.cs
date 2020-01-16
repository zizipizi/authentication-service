using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Models;

namespace Authentication.Host.Services
{
    public interface IUserService
    {
        Task<UserServiceResult> CreateUserAsync(UserCreateModel model, CancellationToken token);

        Task<UserServiceResult> BlockUserAsync(int id, CancellationToken token);

        Task<UserServiceResult> DeleteUserAsync(int id, CancellationToken token);

        Task<UserServiceResult> SignIn(LoginModel model);

    }
}