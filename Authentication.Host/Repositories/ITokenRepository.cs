using System.Threading;
using System.Threading.Tasks;
using NSV.Security.JWT;

namespace Authentication.Host.Repositories
{
    public interface ITokenRepository
    {
        Task<bool> CheckRefreshTokenAsync(TokenModel tokenModel, CancellationToken token);

        Task AddTokensAsync(long userId, TokenModel tokenModel, CancellationToken token);

        Task BlockAllTokensAsync(long id, CancellationToken token);
    }
}