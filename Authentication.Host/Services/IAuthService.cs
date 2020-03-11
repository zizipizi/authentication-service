using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data;
using Authentication.Data.Models.Domain;
using Authentication.Host.Models;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSV.Security.JWT;

namespace Authentication.Host.Services
{
    public interface IAuthService
    {
        Task<Result<HttpStatusCode, BodyTokenModel>> SignIn(LoginModel model, CancellationToken cancellationToken);

        Task<Result<HttpStatusCode, BodyTokenModel>> RefreshToken(BodyTokenModel model, CancellationToken cancellationToken);
    }
}
