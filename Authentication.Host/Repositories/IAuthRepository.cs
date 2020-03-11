using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Models.Domain;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using Nest;
using NSV.Security.JWT;

namespace Authentication.Host.Repositories
{
    public interface IAuthRepository
    {
        Task<Result<AuthRepositoryResult, User>> GetUserByNameAsync(string userName, CancellationToken cancelationToken);

        Task<Result<AuthRepositoryResult>> AddTokensAsync(long userId, TokenModel tokenModel, CancellationToken cancelationToken);
    }
}
