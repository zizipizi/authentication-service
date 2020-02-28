using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Authentication.Tests.RepositoryTests
{
    public class WhenBlockTokens
    {
        [Fact]
        public async Task BlockAllTokens_Ok()
        {
            var authContext = FakeContextFactory.BlockAllTokens_Ok(out var id);
            var logger = new Mock<ILogger<TokenRepository>>().Object;
            var cache = new Mock<IDistributedCache>().Object;
            
            var tokenRepository = new TokenRepository(authContext, logger, cache);

            await tokenRepository.BlockAllTokensAsync(id, CancellationToken.None);

            var allTokens = authContext.RefreshTokens.ToList();

            allTokens[0].IsBlocked.Should().BeTrue();
            allTokens[1].IsBlocked.Should().BeTrue();
            //Assert.True(allTokens[0].IsBlocked);
            //Assert.True(allTokens[1].IsBlocked);
        }
    }
}
