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

        public IEnumerable<UserEntity> GetAllUsers()
        {
            return _context.Users.ToList();
        }


        public async Task<IEnumerable<UserEntity>> GetAllUsersAsync(CancellationToken token)
        {
            return await _context.Users.ToListAsync(token);
        }

        public async Task<UserEntity> GetUserAsync(int id, CancellationToken token)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id, token);
        }

        public UserEntity GetUserByName(string userName)
        {
            var user = _context.Users.SingleOrDefault(obj => obj.Login == userName);
            return user;
        }

        public UserEntity CreateUser(UserEntity user)
        {
            if (!_context.Users.Any())
            {
                var newUser = new UserEntity()
                {
                    Id = 1,
                    Created = DateTime.Today,
                    IsActive = true,
                    Login = user.Login,
                    Password = user.Password,
                    Role = user.Role
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();

                return newUser;
            }
            else
            {
                var newUser = new UserEntity()
                {
                    Id = _context.Users.Max(x => x.Id) + 1,
                    Created = DateTime.Today,
                    IsActive = true,
                    Login = user.Login,
                    Password = user.Password,
                    Role = user.Role
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();

                return newUser;
            }
        }

        public UserEntity DeleteUser(int id)
        {
            return BlockUser(id);
        }

        public UserEntity BlockUser(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);

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
