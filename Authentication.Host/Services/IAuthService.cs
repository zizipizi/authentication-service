using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Models.Domain;
using Authentication.Host.Enums;
using Authentication.Host.Models;
using NSV.Security.JWT;

namespace Authentication.Host.Services
{
    public interface IAuthService
    {
        Task<Result<AuthResult, TokenModel>> SignIn(LoginModel model, CancellationToken token);

        Task<Result<AuthResult, TokenModel>> RefreshToken(TokenModel model, CancellationToken token);
    }
}
