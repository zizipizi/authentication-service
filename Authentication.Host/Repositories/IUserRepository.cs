using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Models.Domain;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using NSV.Security.JWT;

namespace Authentication.Host.Repositories
{
    public interface IUserRepository
    {
        Task<Result<UserRepositoryResult, User>> GetUserByIdAsync(long id, CancellationToken cancellationToken = default);

        Task<Result<UserRepositoryResult>> UpdateUserPasswordAsync(long id, string password, CancellationToken cancellationToken = default);

        Task<Result<UserRepositoryResult, TokenModel>> BlockRefreshTokenAsync(string refreshJti, CancellationToken cancellationToken = default);

        Task<Result<UserRepositoryResult, IEnumerable<TokenModel>>> BlockAllRefreshTokensAsync(long userId, CancellationToken cancellationToken = default);
    }
}
