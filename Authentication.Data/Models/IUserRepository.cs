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

        Task<UserEntity> CreateUserAsync(UserEntity user, CancellationToken token);

        Task<UserEntity> GetUserByNameAsync(string userName, CancellationToken token);

        Task<UserEntity> GetUserByIdAsync(int id, CancellationToken token);

        Task<UserEntity> DeleteUserAsync(int id, CancellationToken token);

        Task<UserEntity> BlockUserAsync(int id, CancellationToken token);

        AccessTokenEntity GetAccessToken(int id);

        RefreshTokenEntity GetRefreshToken(int id);


    }
}
