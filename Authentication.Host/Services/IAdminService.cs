using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Models.Domain;
using Authentication.Host.Models;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;

namespace Authentication.Host.Services
{
    public interface IAdminService
    {
        Task<IEnumerable<User>> GetAllAsync();

        Task<Result<AdminResult, UserInfo>> CreateUserAsync(UserCreateModel model, CancellationToken token);

        Task<Result<AdminResult>> BlockUserAsync(int id, CancellationToken token);

        Task<Result<AdminResult>> DeleteUserAsync(int id, CancellationToken token);
    }
}