using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using Nest;
using NSV.Security.JWT;

namespace Authentication.Host.Repositories
{
    public interface ICacheRepository
    { 
        Task<Result<CacheRepositoryResult>> IsRefreshTokenBlockedAsync(string refreshJti, CancellationToken cancellationToken);

        Task<Result<CacheRepositoryResult>> AddRefreshTokensToBlacklistAsync(IEnumerable<TokenModel> tokens, CancellationToken cancellationToken);

        Task<Result<CacheRepositoryResult>> AddRefreshTokenToBlacklistAsync(TokenModel token, CancellationToken cancellationToken);
    }
}