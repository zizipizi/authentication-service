using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Models;
using Authentication.Host.Results;

namespace Authentication.Host.Services
{
    public interface IUserService
    {
        Task<Result<HttpStatusCode>> SignOutAsync(long id, string refreshJti, CancellationToken cancellationToken = default);

        Task<Result<HttpStatusCode, BodyTokenModel>> ChangePasswordAsync(ChangePassModel model, long id, string accessToken, CancellationToken cancellationToken = default);
    }
}
