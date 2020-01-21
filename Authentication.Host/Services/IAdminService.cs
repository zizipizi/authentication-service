using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Enums;
using Authentication.Host.Models;
using NSV.Security.JWT;

namespace Authentication.Host.Services
{
    public interface IAdminService
    {
        Task<Result<AdminResult>> CreateUserAsync(UserCreateModel model, CancellationToken token);

        Task<Result<AdminResult>> BlockUserAsync(int id, CancellationToken token);

        Task<Result<AdminResult>> DeleteUserAsync(int id, CancellationToken token);

        //Task<Result<AdminResult>> SignIn(LoginModel model, CancellationToken token);

        //Task<Result<AdminResult>> RefreshTokenAsync(TokenModel model, CancellationToken token);
    }
}