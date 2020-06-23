using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Models;
using Authentication.Host.Results;

namespace Authentication.Host.Services
{
    public interface IAuthService
    {
        Task<Result<HttpStatusCode, BodyTokenModel>> SignIn(LoginModel model, CancellationToken cancellationToken = default);

        Task<Result<HttpStatusCode, BodyTokenModel>> RefreshToken(BodyTokenModel model, CancellationToken cancellationToken = default);
    }
}
