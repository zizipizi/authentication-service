using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Models;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using NSV.Security.JWT;

namespace Authentication.Host.Services
{
    public interface IUserService
    {
        Task<Result<UserResult>> SignOutAsync(long id, string refreshJti, CancellationToken token);

        Task<Result<UserResult, TokenModel>> ChangePasswordAsync(ChangePassModel model, long id, string accessToken, CancellationToken token);
    }
}
