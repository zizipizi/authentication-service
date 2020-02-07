﻿using System.Linq;
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

            var tokenRepository = new TokenRepository(authContext, new Mock<ILogger<TokenRepository>>().Object);

            await tokenRepository.BlockAllTokensAsync(1, CancellationToken.None);

            var allTokens = authContext.RefreshTokens.ToList();

            Assert.True(allTokens[0].IsBlocked);
            Assert.True(allTokens[1].IsBlocked);
        }
    }
}
