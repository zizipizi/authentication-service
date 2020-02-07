using System;
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
            
            var refreshJti = "1234567890-0987654321-1234567890";

            var tokenModel = new TokenModel(
                ("asdakasdjaksjdkad", DateTime.Now.AddMinutes(15), "1231123123"),
                ("sdfk;sldkfsdl;fksdf0", DateTime.Now.AddMinutes(30), refreshJti)
            );

            await tokenRepository.CheckRefreshTokenAsync(tokenModel, CancellationToken.None);

            var result = authContext.RefreshTokens.SingleOrDefault(c => c.Jti == refreshJti);

            Assert.NotNull(result);
            Assert.Equal("1234567890-0987654321-1234567890", result.Jti);
        }

        [Fact]
        public async Task CheckRefreshToken_Blocked()
        {
            var authContext = FakeContextFactory.CheckRefreshToken_Blocked();

            var tokenRepository = new TokenRepository(authContext, new Mock<ILogger<TokenRepository>>().Object);
            
            var refreshJti = "1234567890-0987654321-1234567890";
            var tokenModel = new TokenModel(
                ("asdakasdjaksjdkad", DateTime.Now.AddMinutes(15), "1231123123"),
                ("sdfk;sldkfsdl;fksdf0", DateTime.Now.AddMinutes(30), refreshJti)
            );

            Assert.False(await tokenRepository.CheckRefreshTokenAsync(tokenModel, CancellationToken.None));
        }
    }
}
