using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data;
using Authentication.Host.Models;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using NSV.Security.JWT;

namespace Authentication.Host.Services
{
    public interface IUserService
    {
        Task<Result<UserResult>> SignOut(BodyTokenModel tokenModel, string id, string accessToken, CancellationToken token);

        Task<Result<UserResult, TokenModel>> ChangePasswordAsync(ChangePassModel model, string id, string accessToken, CancellationToken token);
    }
}
