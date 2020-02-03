using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Exceptions;
using Authentication.Data.Models;
using Authentication.Data.Models.Domain;
using Authentication.Data.Models.Domain.Translators;
using Authentication.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using NSV.Security.JWT;

namespace Authentication.Host.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthContext _context;
        private readonly ILogger _logger;

        public UserRepository(AuthContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken token)
        { 
            var  users = await _context.Users
                .AsNoTracking()
                .Include(p => p.Roles)
                .ThenInclude(p => p.RoleEn)
                .Select(p => p.ToDomain())
                .ToListAsync(token);

            _logger.LogInformation("TRUE");
            return users;
        }

        public async Task<User> GetUserByNameAsync(string userName, CancellationToken token)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Include(p => p.Roles)
                .ThenInclude(p => p.RoleEn)
                .SingleOrDefaultAsync(obj => obj.Login == userName, token);
            if (user == null)
            {
                _logger.LogError("User not found");
                throw new EntityNotFoundException("User not found");
            }

            return user.ToDomain();
        }

        public async Task<User> GetUserByIdAsync(long id, CancellationToken token)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Include(p => p.Roles)
                .ThenInclude(p => p.RoleEn)
                .SingleOrDefaultAsync(obj => obj.Id == id, token);

            if (user == null)
            {
                _logger.LogError("User not found");
                throw new EntityNotFoundException("User not found");
            }

            return user.ToDomain();
        }

        public async Task CreateUserAsync(User user, CancellationToken token)
        {
            var newUser = user.ToEntity(); 
            await _context.Users.AddAsync(newUser, token);

            if (user.Role.Count() == 1)
            {
                await _context.UsersRoles.AddAsync(new UserRolesEntity
                {
                    UserEn = newUser,
                    RoleEn = _context.Roles.FirstOrDefault(c => c.Role == user.Role.First())
                }, token);
            }
            else
            {
                foreach (var role in user.Role)
                {
                    await _context.UsersRoles.AddAsync(new UserRolesEntity
                    {
                        UserEn = newUser,
                        RoleEn = _context.Roles.FirstOrDefault(c => c.Role == role)
                    }, token);
                }
            }
            await _context.SaveChangesAsync(token);
        }

        public async Task DeleteUserAsync(long id, CancellationToken token)
        {
             await BlockUserAsync(id, token);
        }

        public async Task BlockUserAsync(long id, CancellationToken token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id, token);

            if (user != null)
            {
                user.IsActive = false;

                var refreshTokens = await _context.RefreshTokens.Where(c => c.UserId == user.Id).ToListAsync(token);

                foreach (var i in refreshTokens)
                {
                    i.IsBlocked = true;
                }

                await _context.SaveChangesAsync(token);
            }

            throw new EntityNotFoundException("User not found");
        }

        public async Task CheckRefreshTokenAsync(JwtTokenResult jwtToken, CancellationToken token)
        {
            var refreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(c => c.Jti == jwtToken.RefreshTokenJti, token);

            if (refreshToken.IsBlocked)
                throw new EntityNotFoundException("Token is blocked");
        }

        public async Task UpdateUserPassword(long id, string password, CancellationToken token)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                throw new EntityNotFoundException("User not found");

            user.Password = password;
            await _context.SaveChangesAsync(token);
        }

        public async Task BlockAllTokensAsync(long id, CancellationToken token)
        {
            _context.RefreshTokens.Where(c => c.UserId == id).ToList().ForEach(c => c.IsBlocked = true);

            await _context.SaveChangesAsync(token);
        }

        public async Task AddTokensAsync(JwtTokenResult jwtToken, CancellationToken token)
        {
            var userFromToken = await _context.Users
                .SingleOrDefaultAsync(c => c.Id == long.Parse(jwtToken.UserId), token);


            if (jwtToken.Tokens.RefreshToken == null)
            {
                var accessTokenEntityWithoutRefresh = new AccessTokenEntity
                {
                    Created = DateTime.Now,
                    Exprired = jwtToken.Tokens.AccessToken.Expiration,
                    Token = jwtToken.Tokens.AccessToken.Value,
                    User = userFromToken,
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
                    User = userFromToken
                };

                var accessTokenEntity = new AccessTokenEntity
                {
                    Created = DateTime.UtcNow,
                    Exprired = jwtToken.Tokens.AccessToken.Expiration,
                    Token = jwtToken.Tokens.AccessToken.Value,
                    User = userFromToken,
                    RefreshToken = refreshTokenEntity
                };

                await _context.RefreshTokens.AddAsync(refreshTokenEntity, token);
                await _context.AccessTokens.AddAsync(accessTokenEntity, token);
            }

            await _context.SaveChangesAsync(token);
        }
    }
}
