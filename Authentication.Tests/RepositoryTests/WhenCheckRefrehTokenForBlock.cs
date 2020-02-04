using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Exceptions;
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
            var logger = new Mock<ILogger<UserRepository>>().Object;

            var userRepository = new UserRepository(authContext, logger);

            var jti = "1234567890-0987654321-1234567890";

            var token = new JwtTokenResult(refreshTokenJti: jti);

            await userRepository.CheckRefreshTokenAsync(token, CancellationToken.None);

            var result = authContext.RefreshTokens.SingleOrDefault(c => c.Jti == jti);

            Assert.NotNull(result);
            Assert.Equal("1234567890-0987654321-1234567890", result.Jti);
        }

        [Fact]
        public async Task CheckRefreshToken_Blocked()
        {
            var authContext = FakeContextFactory.CheckRefreshToken_Blocked();
            var logger = new Mock<ILogger<UserRepository>>().Object;

            var userRepository = new UserRepository(authContext, logger);

            var jti = "1234567890-0987654321-1234567890";

            var token = new JwtTokenResult(refreshTokenJti: jti);

            var ex =await Assert.ThrowsAsync<EntityNotFoundException>(async () => 
                await userRepository.CheckRefreshTokenAsync(token, CancellationToken.None));

            Assert.Equal("Token is blocked or not found", ex.Message);
        }
    }
}
