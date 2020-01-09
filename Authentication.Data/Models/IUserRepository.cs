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

        Task<UserEntity> CreateUser(UserEntity user, CancellationToken token);

        Task<UserEntity> GetUserByName(string userName, CancellationToken token);

        Task<UserEntity> DeleteUser(int id, CancellationToken token);

        Task<UserEntity> BlockUser(int id, CancellationToken token);

        AccessTokenEntity GetAccessToken(int id);

        RefreshTokenEntity GetRefreshToken(int id);


    }
}
