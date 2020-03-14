using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using NSV.Security.JWT;

namespace Authentication.Host.Repositories
{
    public interface ICacheRepository
    { 
        Task<Result<CacheRepositoryResult>> IsRefreshTokenBlockedAsync(string refreshJti, CancellationToken cancellationToken = default);

        Task<Result<CacheRepositoryResult>> AddRefreshTokensToBlacklistAsync(IEnumerable<TokenModel> tokens, CancellationToken cancellationToken = default);

        Task<Result<CacheRepositoryResult>> AddRefreshTokenToBlacklistAsync(TokenModel token, CancellationToken cancellationToken = default);
    }
}