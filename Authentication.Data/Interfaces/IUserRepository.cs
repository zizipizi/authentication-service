using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Models.Data;
using Authentication.Data.Models.Entities;


namespace Authentication.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken token);

        Task CreateUserAsync(UserEntity user, CancellationToken token);

        Task<User> GetUserByNameAsync(string userName, CancellationToken token);

        Task<User> GetUserByIdAsync(int id, CancellationToken token);

        Task DeleteUserAsync(int id, CancellationToken token);

        Task BlockUserAsync(int id, CancellationToken token);

        AccessTokenEntity GetAccessToken(int id);

        RefreshTokenEntity GetRefreshToken(int id);


    }
}
