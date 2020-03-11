using System;
using System.Collections.Generic;
using System.Linq;
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
        Task<Result<AdminRepositoryResult, User>> CreateUserAsync(User user, CancellationToken cancellationToken);

        Task<Result<AdminRepositoryResult, IEnumerable<TokenModel>>> BlockUserAsync(long id, CancellationToken cancellationToken);

        Task<Result<AdminRepositoryResult>> DeleteUserAsync(long id, CancellationToken cancellationToken);

        Task<Result<AdminRepositoryResult, IEnumerable<User>>> GetAllUsersAsync(CancellationToken cancellationToken);
    }
}
