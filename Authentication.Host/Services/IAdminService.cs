using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Models.Domain;
using Authentication.Host.Models;
using Authentication.Host.Results;

namespace Authentication.Host.Services
{
    public interface IAdminService
    {
        Task<Result<HttpStatusCode, IEnumerable<User>>> GetAllUsersAsync(CancellationToken cancellationToken = default);

        Task<Result<HttpStatusCode, UserInfo>> CreateUserAsync(UserCreateModel model, CancellationToken cancellationToken = default);

        Task<Result<HttpStatusCode>> BlockUserAsync(long id, CancellationToken cancellationToken = default);

        Task<Result<HttpStatusCode>> DeleteUserAsync(long id, CancellationToken cancellationToken = default);
    }
}