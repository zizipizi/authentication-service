using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using NSV.Security.JWT;
using Xunit;

namespace Authentication.Tests.RepositoryTests
{
    public class WhenCheckRefrehTokenForBlock
    {
        [Fact]
        public async Task CheckRefreshToken_Ok()
        {
            var authContext = FakeContextFactory.CheckRefreshToken_Ok();
            var logger = new Mock<ILogger<TokenRepository>>().Object;

            var tokenRepository = new TokenRepository(authContext, logger);
            
            var jti = "1234567890-0987654321-1234567890";

            var token = new JwtTokenResult(refreshTokenJti: jti);

            await tokenRepository.CheckRefreshTokenAsync(token, CancellationToken.None);

            var result = authContext.RefreshTokens.SingleOrDefault(c => c.Jti == jti);

            Assert.NotNull(result);
            Assert.Equal("1234567890-0987654321-1234567890", result.Jti);
        }

        [Fact]
        public async Task CheckRefreshToken_Blocked()
        {
            var authContext = FakeContextFactory.CheckRefreshToken_Blocked();

            var tokenRepository = new TokenRepository(authContext, new Mock<ILogger<TokenRepository>>().Object);
            
            var jti = "1234567890-0987654321-1234567890";

            var token = new JwtTokenResult(refreshTokenJti: jti);

            Assert.False(await tokenRepository.CheckRefreshTokenAsync(token, CancellationToken.None));
        }
    }
}
