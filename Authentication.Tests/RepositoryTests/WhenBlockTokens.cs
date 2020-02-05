using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Repositories;
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
            var authContext = FakeContextFactory.BlockAllTokens_Ok();
            var logger = new Mock<ILogger<UserRepository>>().Object;

            var userRepository = new UserRepository(authContext, logger);

            await userRepository.BlockAllTokensAsync(1, CancellationToken.None);

            var allTokens = authContext.RefreshTokens.ToList();

            Assert.True(allTokens[0].IsBlocked);
            Assert.True(allTokens[1].IsBlocked);
        }
    }
}
