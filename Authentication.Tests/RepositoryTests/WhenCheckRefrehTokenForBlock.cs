using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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
            var authContext = FakeContextFactory.CheckRefreshToken_Blocked();
            var cache = new Mock<IDistributedCache>();
            var tokenRepository = new TokenRepository(authContext, new Mock<ILogger<TokenRepository>>().Object, cache.Object);
            cache.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new byte['d']));

            var refreshJti = "1234567890-0987654321-1234567890";
            var tokenModel = new TokenModel(
                ("asdakasdjaksjdkad", DateTime.Now.AddMinutes(15), "1231123123"),
                ("sdfk;sldkfsdl;fksdf0", DateTime.Now.AddMinutes(30), refreshJti)
            );

            var result = await tokenRepository.IsRefreshTokenBlockedAsync(refreshJti, CancellationToken.None);
            result.Should().BeTrue();

            //Assert.True(await tokenRepository.IsRefreshTokenBlockedAsync(refreshJti, CancellationToken.None));
        }

        [Fact]
        public async Task CheckRefreshToken_Blocked()
        {
            var authContext = FakeContextFactory.CheckRefreshToken_Blocked();
            var cache = new Mock<IDistributedCache>();
            var tokenRepository = new TokenRepository(authContext, new Mock<ILogger<TokenRepository>>().Object, cache.Object);
            cache.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult((byte[])null));

            var refreshJti = "1234567890-0987654321-1234567890";
            var tokenModel = new TokenModel(
                ("asdakasdjaksjdkad", DateTime.Now.AddMinutes(15), "1231123123"),
                ("sdfk;sldkfsdl;fksdf0", DateTime.Now.AddMinutes(30), refreshJti)
            );

            var result = await tokenRepository.IsRefreshTokenBlockedAsync(refreshJti, CancellationToken.None);
            result.Should().BeFalse();

            //Assert.False(await tokenRepository.IsRefreshTokenBlockedAsync(refreshJti, CancellationToken.None));
        }
    }
}
