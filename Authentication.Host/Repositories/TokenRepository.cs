using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Exceptions;
using Authentication.Data.Models;
using Authentication.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using NSV.Security.JWT;

namespace Authentication.Host.Repositories
{
    public class TokenRepository : RepositoryBase, ITokenRepository
    {
        private readonly IDistributedCache _cache;
        public TokenRepository(AuthContext context, ILogger<TokenRepository> logger, IDistributedCache cache) 
            : base(context, logger)
        {
            _cache = cache;
        }

        public async Task<bool> IsRefreshTokenBlockedAsync(string refreshJti, CancellationToken token)
        {
            
            var cacheRefreshToken = await _cache.GetAsync($"blacklist:{refreshJti}", token);

            return cacheRefreshToken != null;

            // Проверка в БД была заглушкой, теперь проверяем только в кэше. На всякий случай оставил проверку в БД закомменченной. Мб нужна будет

            //var DbRefreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(c => c.Jti == tokenModel.RefreshToken.Jti, token);
            //return DbRefreshToken != null && !DbRefreshToken.IsBlocked;
        }

        //Block when user signout. Block only current refresh token.
        //And all access with same refresh jti???
        public async Task BlockRefreshTokenAsync(string refreshJti, CancellationToken cancellationToken)
        {
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(c => c.Jti == refreshJti, cancellationToken);

            if (!await IsRefreshTokenBlockedAsync(refreshJti, cancellationToken))
            {
                refreshToken.IsBlocked = true;

                await _cache.SetStringAsync($"blacklist:{refreshJti}", refreshJti, new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = refreshToken.Expired
                }, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        //Block all when user change password or admin block user
        public async Task BlockAllTokensAsync(long id, CancellationToken cancellationToken)
        {
            var tokens = _context.RefreshTokens.Where(c => !c.IsBlocked && c.UserId == id && c.Expired > DateTime.UtcNow)
                .ToList();
            
            foreach (var token in tokens)
            {
                _cache.SetString($"blacklist:{token.Jti}", token.Jti, new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = token.Expired
                });

                token.IsBlocked = true;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task AddTokensAsync(long userId, TokenModel tokenModel, CancellationToken token)
        {
            var refreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(c => c.Jti == tokenModel.RefreshToken.Jti, token);

            if (refreshToken != null)
            {
                var accessTokenEntityWithoutRefresh = new AccessTokenEntity
                {
                    Created = DateTime.Now,
                    Exprired = tokenModel.AccessToken.Expiration,
                    Token = tokenModel.AccessToken.Value,
                    UserId = userId,
                    RefreshToken = refreshToken
                };

                await _context.AccessTokens.AddAsync(accessTokenEntityWithoutRefresh, token);
            }
            else
            {
                var refreshTokenEntity = new RefreshTokenEntity
                {
                    Token = tokenModel.RefreshToken.Value,
                    Created = DateTime.UtcNow,
                    Expired = tokenModel.RefreshToken.Expiration,
                    Jti = tokenModel.RefreshToken.Jti,
                    IsBlocked = false,
                    UserId = userId,
                };


                var accessTokenEntity = new AccessTokenEntity
                {
                    Created = DateTime.UtcNow,
                    Exprired = tokenModel.AccessToken.Expiration,
                    Token = tokenModel.AccessToken.Value,
                    UserId = userId,
                    RefreshToken = refreshTokenEntity
                };

                await _context.RefreshTokens.AddAsync(refreshTokenEntity, token);
                await _context.AccessTokens.AddAsync(accessTokenEntity, token);
            }

            await _context.SaveChangesAsync(token);
        }
    }
}