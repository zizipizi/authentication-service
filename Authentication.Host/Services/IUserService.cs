using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Models;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using Microsoft.AspNetCore.Mvc;
using NSV.Security.JWT;

namespace Authentication.Host.Services
{
    public interface IUserService
    {
        Task<Result<HttpStatusCode>> SignOutAsync(long id, string refreshJti, CancellationToken token);

        Task<Result<HttpStatusCode, BodyTokenModel>> ChangePasswordAsync(ChangePassModel model, long id, string accessToken, CancellationToken token);
    }
}
