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
using Microsoft.EntityFrameworkCore;

namespace Authentication.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthContext _context;

        public UserRepository(AuthContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserInfo>> GetAllUsersAsync(CancellationToken token)
        {
            var users = await _context.Users.Select(i => new UserInfo()
            {
                Id = i.Id,
                Login = i.Login,
                IsActive = i.IsActive,
                Role = i.Role
            }).ToListAsync(token);

            return users;
        }

        public async Task<User> GetUserByNameAsync(string userName, CancellationToken token)
        {
            var user = await _context.Users.SingleOrDefaultAsync(obj => obj.Login == userName, token);
            if (user == null)
                throw new EntityNotFoundException("User not found");

            return user.ToDomain();
        }

        public async Task<User> GetUserByIdAsync(long id, CancellationToken token)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(obj => obj.Id == id, token);
            if (user == null)
                throw new EntityNotFoundException("User not found");

            return user.ToDomain();
        }

        public async Task CreateUserAsync(User user, CancellationToken token)
        {
            var newUser = user.ToEntity(); 
            await _context.Users.AddAsync(newUser, token);

            await _context.SaveChangesAsync(token);
        }

        public async Task DeleteUserAsync(long id, CancellationToken token)
        {
             await BlockUserAsync(id, token);
        }

        public async Task BlockUserAsync(long id, CancellationToken token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id, token);
            if (user == null)
                throw new EntityNotFoundException("User not found");

            user.IsActive = false;
            await _context.SaveChangesAsync(token);
        }

        public AccessTokenEntity GetAccessToken(int id)
        {
            throw new NotImplementedException();
        }

        public RefreshTokenEntity GetRefreshToken(int id)
        {
            throw new NotImplementedException();
        }
    }
}
