using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Exceptions;
using Authentication.Data.Models;
using Authentication.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSV.Security.JWT;

namespace Authentication.Host.Repositories
{
    public class TokenRepository : RepositoryBase, ITokenRepository
    {
        public TokenRepository(AuthContext context, ILogger<TokenRepository> logger) 
            : base(context, logger)
        {
        }

        public async Task<bool> CheckRefreshTokenAsync(TokenModel tokenModel, CancellationToken token)
        {
            var refreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(c => c.Jti == tokenModel.RefreshToken.Jti, token);

            return refreshToken != null && !refreshToken.IsBlocked;
        }

        public async Task BlockAllTokensAsync(long id, CancellationToken token)
        {
            _context.RefreshTokens.Where(c => !c.IsBlocked && c.UserId == id)
                .ToList()
                .ForEach(c => c.IsBlocked = true);

            await _context.SaveChangesAsync(token);
        }

        public async Task AddTokensAsync(long userId, TokenModel tokenModel, CancellationToken token)
        {
            if (tokenModel.RefreshToken == null)
            {
                var accessTokenEntityWithoutRefresh = new AccessTokenEntity
                {
                    Created = DateTime.Now,
                    Exprired = tokenModel.AccessToken.Expiration,
                    Token = tokenModel.AccessToken.Value,
                    UserId = userId,
                    RefreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(c => c.Jti == tokenModel.RefreshToken.Jti, token)
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