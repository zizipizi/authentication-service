using System;
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
using static Authentication.Host.Results.ResultExtensions;

namespace Authentication.Host.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AuthContext _context;
        private readonly ILogger _logger;

        public AdminRepository(AuthContext context, ILogger<AdminRepository> logger = null)
        {
            _context = context;
            _logger = logger ?? new NullLogger<AdminRepository>();
        }
        
        public async Task<Result<AdminRepositoryResult, User>> CreateUserAsync(User user, CancellationToken cancellationToken)
        {
            try
            {
                var newUser = user.ToEntity();
                var userExist = await _context.Users.AnyAsync(c => c.Login == user.Login, cancellationToken);

                if (userExist)
                    return new Result<AdminRepositoryResult, User>(AdminRepositoryResult.UserExist);

                await _context.Users.AddAsync(newUser, cancellationToken);

                var roles = await _context.Roles
                    .Where(x => user.Role.Contains(x.Role))
                    .Select(x => x.Id)
                    .ToArrayAsync(cancellationToken);

                var userRoles = roles
                    .Select(roleId => new UserRolesEntity
                    {
                        UserEn = newUser,
                        RoleId = roleId
                    });

                await _context.UsersRoles.AddRangeAsync(userRoles, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                return new Result<AdminRepositoryResult, User>(AdminRepositoryResult.Ok, newUser.ToDomain());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Result<AdminRepositoryResult, User>(AdminRepositoryResult.Error);
            }
        }

        public async Task<Result<AdminRepositoryResult, IEnumerable<TokenModel>>> BlockUserAsync(long id, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

                if (user == null)
                    return new Result<AdminRepositoryResult, IEnumerable<TokenModel>>(AdminRepositoryResult.UserNotFound);

                user.IsActive = false;

                var userTokens = await _context.RefreshTokens
                    .AsNoTracking()
                    .Where(c => c.UserId == id && c.IsBlocked != true && c.Expired > DateTime.UtcNow)
                    .Select(c => c.toTokenModel()).ToListAsync(cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                return new Result<AdminRepositoryResult, IEnumerable<TokenModel>>(AdminRepositoryResult.Ok, userTokens);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Result<AdminRepositoryResult, IEnumerable<TokenModel>>(AdminRepositoryResult.Error);
            }
        }

        public Task<Result<AdminRepositoryResult>> DeleteUserAsync(long id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<AdminRepositoryResult, Dictionary<string, DateTime>>> GetTokensForBlock(long id, CancellationToken cancellationToken)
        {
            try
            {
                var tokens = await _context.RefreshTokens
                    .Where(c => c.Id == id && c.Expired > DateTime.UtcNow)
                    .ToListAsync(cancellationToken);

                var tokenDict = new Dictionary<string, DateTime>();

                foreach (var token in tokens)
                {
                    tokenDict.Add(token.Jti, token.Expired);
                }

                return new Result<AdminRepositoryResult, Dictionary<string, DateTime>>(AdminRepositoryResult.Ok, tokenDict);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Result<AdminRepositoryResult, Dictionary<string, DateTime>>(AdminRepositoryResult.Error);
            }
        }

        public async Task<Result<AdminRepositoryResult, IEnumerable<User>>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            try
            {
                var users = await _context.Users
                    .AsNoTracking()
                    .Include(s => s.Roles)
                    .ThenInclude(s => s.RoleEn)
                    .Select(u => u.ToDomain())
                    .ToListAsync(cancellationToken);

                return new Result<AdminRepositoryResult, IEnumerable<User>>(AdminRepositoryResult.Ok, users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Result<AdminRepositoryResult, IEnumerable<User>>(AdminRepositoryResult.Error);
            }
        }
    }
}
