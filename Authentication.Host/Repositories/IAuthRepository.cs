using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Models.Domain;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using NSV.Security.JWT;

namespace Authentication.Host.Repositories
{
    public interface IAuthRepository
    {
        Task<Result<AuthRepositoryResult, User>> GetUserByNameAsync(string userName, CancellationToken cancellationToken = default);

        Task<Result<AuthRepositoryResult>> AddTokensAsync(long userId, TokenModel tokenModel, string ipAddress = null, CancellationToken cancellationToken = default);
    }
}
