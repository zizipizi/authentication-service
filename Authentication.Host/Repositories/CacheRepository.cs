using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Models;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSV.Security.JWT;

namespace Authentication.Host.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        private readonly AuthContext _context;
        private readonly IDistributedCache _cache;
        private readonly ILogger _logger;

        public CacheRepository(IDistributedCache cache,AuthContext context, ILogger<CacheRepository> logger = null)
        {
            _cache = cache;
            _context = context;
            _logger = logger ?? new NullLogger<CacheRepository>();
        }
        public async Task<Result<CacheRepositoryResult>> IsRefreshTokenBlockedAsync(string refreshJti, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _cache.GetAsync($"blacklist:{refreshJti}", cancellationToken);

                return result != null 
                    ? new Result<CacheRepositoryResult>(CacheRepositoryResult.IsBlocked) 
                    : new Result<CacheRepositoryResult>(CacheRepositoryResult.IsNotBlocked);
            }
            catch (Exception ex)
            { 
                _logger.LogError(ex.Message);
                return new Result<CacheRepositoryResult>(CacheRepositoryResult.Error);
            }
        }

        public async Task<Result<CacheRepositoryResult>> AddRefreshTokensToBlacklistAsync(IEnumerable<TokenModel> tokens, CancellationToken cancellationToken)
        {
            try
            {
                foreach (var token in tokens)
                {
                    await _cache.SetStringAsync($"blacklist:{token.RefreshToken.Value}", token.RefreshToken.Value,
                        new DistributedCacheEntryOptions
                        {
                            AbsoluteExpiration = token.RefreshToken.Expiration
                        }, cancellationToken);
                }

                return new Result<CacheRepositoryResult>(CacheRepositoryResult.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Result<CacheRepositoryResult>(CacheRepositoryResult.Error);
            }
        }

        public async Task<Result<CacheRepositoryResult>> AddRefreshTokenToBlacklistAsync(TokenModel token, CancellationToken cancellationToken)
        {
            try
            {
                await _cache.SetStringAsync($"blacklist:{token.RefreshToken.Jti}", token.RefreshToken.Jti, new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = token.RefreshToken.Expiration
                }, cancellationToken);
                return new Result<CacheRepositoryResult>(CacheRepositoryResult.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Result<CacheRepositoryResult>(CacheRepositoryResult.Error);
            }
        }


    }
}
