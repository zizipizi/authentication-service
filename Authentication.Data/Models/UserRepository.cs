using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Data.Models
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthContext _context;

        public UserRepository(AuthContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserEntity>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }


        public async Task<IEnumerable<UserEntity>> GetAllUsersAsync(CancellationToken token)
        {
            return await _context.Users.ToListAsync(token);
        }

        public async Task<UserEntity> GetUserAsync(int id, CancellationToken token)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id, token);
        }

        public async Task<UserEntity> GetUserByName(string userName, CancellationToken token)
        {
            var user = await _context.Users.SingleOrDefaultAsync(obj => obj.Login == userName, token);
            return user;
        }


        public async Task<UserEntity> CreateUser(UserEntity user, CancellationToken token)
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

                return newUser;
        }

        public async Task<UserEntity> DeleteUser(int id, CancellationToken token)
        {
            return await BlockUser(id, token);
        }

        public async Task<UserEntity> BlockUser(int id, CancellationToken token)
        {

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id, token);
            user.IsActive = false;

            return user;
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
