using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NSV.Security.JWT;

namespace Authentication.Host.Repositories
{
    public interface ITokenRepository
    {
        Task<bool> IsRefreshTokenBlockedAsync(string refreshJti, CancellationToken cancellationToken);

        Task AddTokensAsync(long userId, TokenModel tokenModel, CancellationToken cancellationToken);

        Task BlockAllTokensAsync(long id, CancellationToken cancellationToken);

        Task BlockRefreshTokenAsync(string refreshJti, CancellationToken cancellationToken);
    }
}