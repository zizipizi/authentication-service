using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Models.Domain;
using Authentication.Data.Models.Entities;
using NSV.Security.JWT;

namespace Authentication.Host.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken token);

        Task CreateUserAsync(User user, CancellationToken token);

        Task<User> GetUserByNameAsync(string userName, CancellationToken token);

        Task<User> GetUserByIdAsync(long id, CancellationToken token);

        Task DeleteUserAsync(long id, CancellationToken token);

        Task BlockUserAsync(long id, CancellationToken token);

        Task CheckRefreshTokenAsync(JwtTokenResult jwtToken, CancellationToken token);

        Task AddTokensAsync(JwtTokenResult jwtToken, CancellationToken token);

        Task UpdateUserPassword(long id, string password, CancellationToken token);

        Task BlockAllTokensAsync(long id, CancellationToken token);

    }
}
