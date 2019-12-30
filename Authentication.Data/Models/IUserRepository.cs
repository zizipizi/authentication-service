using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Authentication.Data.Models
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserEntity>> GetAllUsersAsync(CancellationToken token);

        Task<UserEntity> GetUserAsync(int id, CancellationToken token);

        UserEntity CreateUser(UserEntity user);

        UserEntity GetUserByName(string userName);

        UserEntity DeleteUser(int id);

        UserEntity BlockUser(int id);

        AccessTokenEntity GetAccessToken(int id);

        RefreshTokenEntity GetRefreshToken(int id);


    }
}
