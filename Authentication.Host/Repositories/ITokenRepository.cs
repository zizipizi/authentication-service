using System.Threading;
using System.Threading.Tasks;
using NSV.Security.JWT;

namespace Authentication.Host.Repositories
{
    public interface ITokenRepository
    {
        Task<bool> CheckRefreshTokenAsync(JwtTokenResult jwtToken, CancellationToken token);

        Task AddTokensAsync(JwtTokenResult jwtToken, CancellationToken token);

        Task BlockAllTokensAsync(long id, CancellationToken token);
    }
}