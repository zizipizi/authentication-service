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

        public async Task<bool> CheckRefreshTokenAsync(JwtTokenResult jwtToken, CancellationToken token)
        {
            var refreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(c => c.Jti == jwtToken.RefreshTokenJti, token);

            return refreshToken != null && !refreshToken.IsBlocked;
        }

        public async Task BlockAllTokensAsync(long id, CancellationToken token)
        {
            _context.RefreshTokens.Where(c => !c.IsBlocked && c.UserId == id)
                .ToList()
                .ForEach(c => c.IsBlocked = true);

            await _context.SaveChangesAsync(token);
        }

        public async Task AddTokensAsync(JwtTokenResult jwtToken, CancellationToken token)
        {
            var id = long.Parse(jwtToken.UserId);

            if (jwtToken.Tokens.RefreshToken == null)
            {
                var accessTokenEntityWithoutRefresh = new AccessTokenEntity
                {
                    Created = DateTime.Now,
                    Exprired = jwtToken.Tokens.AccessToken.Expiration,
                    Token = jwtToken.Tokens.AccessToken.Value,
                    UserId = id,
                    RefreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(c => c.Jti == jwtToken.RefreshTokenJti, token)
                };

                await _context.AccessTokens.AddAsync(accessTokenEntityWithoutRefresh, token);
            }
            else
            {
                var refreshTokenEntity = new RefreshTokenEntity
                {
                    Token = jwtToken.Tokens.RefreshToken.Value,
                    Created = DateTime.UtcNow,
                    Expired = jwtToken.Tokens.RefreshToken.Expiration,
                    Jti = jwtToken.RefreshTokenJti,
                    IsBlocked = false,
                    UserId = id,
                };


                var accessTokenEntity = new AccessTokenEntity
                {
                    Created = DateTime.UtcNow,
                    Exprired = jwtToken.Tokens.AccessToken.Expiration,
                    Token = jwtToken.Tokens.AccessToken.Value,
                    UserId = id,
                    RefreshToken = refreshTokenEntity
                };

                await _context.RefreshTokens.AddAsync(refreshTokenEntity, token);
                await _context.AccessTokens.AddAsync(accessTokenEntity, token);
            }

            await _context.SaveChangesAsync(token);
        }
    }
}