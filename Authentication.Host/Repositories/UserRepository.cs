using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Exceptions;
using Authentication.Data.Models;
using Authentication.Data.Models.Domain;
using Authentication.Data.Models.Domain.Translators;
using Authentication.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Authentication.Host.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        private readonly ITokenRepository _tokenRepository;

        public UserRepository(ITokenRepository tokenRepository, AuthContext context, ILogger<UserRepository> logger)
            : base(context, logger)
        {
            _tokenRepository = tokenRepository;
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

        public async Task<long> CreateUserAsync(User user, CancellationToken token)
        {
            var newUser = user.ToEntity();
            var userExist = await _context.Users.AnyAsync(c => c.Login == user.Login, token);

            if (userExist)
                throw new EntityNotFoundException("User already exist");

            await _context.Users.AddAsync(newUser, token);

            var roles = await _context.Roles
                .Where(x => user.Role.Contains(x.Role))
                .Select(x => x.Id)
                .ToArrayAsync(token);

            var userRoles = roles
                .Select(roleId => new UserRolesEntity
                {
                    UserEn = newUser,
                    RoleId = roleId
                });

            await _context.UsersRoles.AddRangeAsync(userRoles, token);

            await _context.SaveChangesAsync(token);

            return newUser.Id;
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

                await _tokenRepository.BlockAllTokensAsync(user.Id, token);

                await _context.SaveChangesAsync(token);
            }
            else
            {
                throw new EntityNotFoundException("User not found");
            }
        }

        public async Task UpdateUserPassword(long id, string password, CancellationToken token)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                throw new EntityNotFoundException("User not found");

            user.Password = password;
            await _context.SaveChangesAsync(token);
        }
    }
}
