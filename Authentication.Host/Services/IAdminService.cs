using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Models.Domain;
using Authentication.Host.Models;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;

namespace Authentication.Host.Services
{
    public interface IAdminService
    {
        Task<Result<HttpStatusCode, IEnumerable<User>>> GetAllUsersAsync(CancellationToken cancellationToken);

        Task<Result<HttpStatusCode, UserInfo>> CreateUserAsync(UserCreateModel model, CancellationToken cancellationToken);

        Task<Result<HttpStatusCode>> BlockUserAsync(long id, CancellationToken cancellationToken);

        Task<Result<HttpStatusCode>> DeleteUserAsync(long id, CancellationToken cancellationToken);
    }
}