using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Interfaces;
using Authentication.Data.Models;
using Authentication.Data.Models.Data;
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

        public async Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken token)
        {
            var users = await _context.Users.Select(i => new User()
            {
                Id = i.Id,
                Login = i.Login,
                IsActive = i.IsActive,
                Password = i.Password,
                Role = i.Role
            }).ToListAsync(token);

            return users;
        }

        public async Task<User> GetUserByNameAsync(string userName, CancellationToken token)
        {
            var user = await _context.Users.Select(i => new User()
            {
                Login = i.Login
            }).SingleOrDefaultAsync(obj => obj.Name == userName, token);

            return user;
        }

        public async Task<User> GetUserByIdAsync(int id, CancellationToken token)
        {
            var user = await _context.Users.Select(i => new User()
            {
                Id = i.Id
            }).FirstOrDefaultAsync(obj => obj.Id == id, token);
            return user;
        }

        public async Task CreateUserAsync(UserEntity user, CancellationToken token)
        {
            var newUser = new UserEntity()
                {
                    Created = DateTime.Today,
                    IsActive = true,
                    Login = user.Login,
                    Password = user.Password,
                    Role = user.Role
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync(token);
        }

        public async Task DeleteUserAsync(int id, CancellationToken token)
        {
             await BlockUserAsync(id, token);
        }

        public async Task BlockUserAsync(int id, CancellationToken token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id, token);

            user.IsActive = false;
            _context.Update(user);
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
