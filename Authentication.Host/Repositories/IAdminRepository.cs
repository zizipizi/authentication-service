using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Models.Domain;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using NSV.Security.JWT;

namespace Authentication.Host.Repositories
{
    public interface IAdminRepository
    {
        Task<Result<AdminRepositoryResult, UserInfo>> CreateUserAsync(User user, CancellationToken cancellationToken = default);

        Task<Result<AdminRepositoryResult, IEnumerable<TokenModel>>> BlockUserAsync(long id, CancellationToken cancellationToken = default);

        Task<Result<AdminRepositoryResult, IEnumerable<TokenModel>>> DeleteUserAsync(long id, CancellationToken cancellationToken = default);

        Task<Result<AdminRepositoryResult, IEnumerable<User>>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    }
}
