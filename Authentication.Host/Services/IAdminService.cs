using System.Threading;
using System.Threading.Tasks;
using Authentication.Data;
using Authentication.Host.Models;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using NSV.Security.JWT;

namespace Authentication.Host.Services
{
    public interface IAdminService
    {
        Task<Result<AdminResult>> CreateUserAsync(UserCreateModel model, CancellationToken token);

        Task<Result<AdminResult>> BlockUserAsync(int id, CancellationToken token);

        Task<Result<AdminResult>> DeleteUserAsync(int id, CancellationToken token);
    }
}