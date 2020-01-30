﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data;
using Authentication.Data.Models.Domain;
using Authentication.Host.Models;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using NSV.Security.JWT;

namespace Authentication.Host.Services
{
    public interface IAuthService
    {
        Task<Result<AuthResult, TokenModel>> SignIn(LoginModel model, CancellationToken token);

        Task<Result<AuthResult, TokenModel>> RefreshToken(BodyTokenModel model, CancellationToken token);
    }
}