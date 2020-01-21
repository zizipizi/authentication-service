using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Models.Domain;
using Authentication.Data.Models.Entities;

namespace Authentication.Data.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserInfo>> GetAllUsersAsync(CancellationToken token);

        Task CreateUserAsync(User user, CancellationToken token);

        Task<User> GetUserByNameAsync(string userName, CancellationToken token);

        Task<User> GetUserByIdAsync(long id, CancellationToken token);

        Task DeleteUserAsync(long id, CancellationToken token);

        Task BlockUserAsync(long id, CancellationToken token);

        AccessTokenEntity GetAccessToken(int id);

        RefreshTokenEntity GetRefreshToken(int id);
    }
}
