using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Exceptions;
using Authentication.Data.Models;
using Authentication.Data.Models.Domain;
using Authentication.Data.Models.Domain.Translators;
using Authentication.Data.Models.Entities;
using Authentication.Host.Models.Translators;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSV.Security.JWT;

namespace Authentication.Host.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthContext _context;
        private readonly ILogger _logger;
        public UserRepository(AuthContext context, ILogger<UserRepository> logger = null)
        {
            _context = context;
            _logger = logger ?? new NullLogger<UserRepository>();
        }

        public async Task<Result<UserRepositoryResult, User>> GetUserByIdAsync(long id, CancellationToken token)
        {
            UserEntity user;
            try
            {
                 user = await _context.Users
                    .AsNoTracking()
                    .Include(p => p.Roles)
                    .ThenInclude(p => p.RoleEn)
                    .SingleOrDefaultAsync(obj => obj.Id == id, token);

                 return user == null 
                     ? new Result<UserRepositoryResult, User>(UserRepositoryResult.UserNotFound) 
                     : new Result<UserRepositoryResult, User>(UserRepositoryResult.Ok, user.ToDomain());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Result<UserRepositoryResult, User>(UserRepositoryResult.Error);
            }
        }

        public async Task<Result<UserRepositoryResult, TokenModel>> BlockRefreshTokenAsync(string refreshJti, CancellationToken cancellationToken)
        {
            try
            {
                var token = await _context.RefreshTokens
                    .Include(c => c.AccessToken)
                    .FirstOrDefaultAsync(c => c.Jti == refreshJti, cancellationToken);

                token.IsBlocked = true;
                await _context.SaveChangesAsync(cancellationToken);

                return new Result<UserRepositoryResult, TokenModel>(UserRepositoryResult.Ok, token.toTokenModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Result<UserRepositoryResult, TokenModel>(UserRepositoryResult.Error);
            }
        }

        public async Task<Result<UserRepositoryResult, IEnumerable<TokenModel>>> BlockAllRefreshTokensAsync(long userId, CancellationToken cancellationToken)
        {
            try
            {
                var tokens = await _context.RefreshTokens
                    .Where(c => !c.IsBlocked && c.UserId == userId && c.Expired > DateTime.UtcNow)
                    .ToListAsync(cancellationToken);

                var tokenList = new List<TokenModel>();

                foreach (var token in tokens)
                {
                    token.IsBlocked = true;
                    tokenList.Add(token.toTokenModel());
                }

                await _context.SaveChangesAsync(cancellationToken);

                return new Result<UserRepositoryResult, IEnumerable<TokenModel>>(UserRepositoryResult.Ok, tokenList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Result<UserRepositoryResult, IEnumerable<TokenModel>>(UserRepositoryResult.Error);
            }
        }

        public async Task<Result<UserRepositoryResult>> UpdateUserPasswordAsync(long id, string password, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                user.Password = password;
                await _context.SaveChangesAsync(cancellationToken);

                return new Result<UserRepositoryResult>(UserRepositoryResult.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Result<UserRepositoryResult>(UserRepositoryResult.Error);
            }

        }
    }
}
