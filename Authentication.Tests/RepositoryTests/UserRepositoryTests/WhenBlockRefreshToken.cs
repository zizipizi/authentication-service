using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Repositories;
using Authentication.Host.Results.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Authentication.Tests.RepositoryTests.UserRepositoryTests
{
    public class WhenBlockRefreshToken
    {
        [Fact]
        public async Task WhenBlockRefreshToken_Ok()
        {
            var authContext = FakeContextFactory.BlockRefreshToken_Ok(out var refreshJti);

            var userRepository = new UserRepository(authContext);

            var blockResult = await userRepository.BlockRefreshTokenAsync(refreshJti, CancellationToken.None);
            var blockedRefreshToken = await authContext.RefreshTokens.FirstOrDefaultAsync(c => c.Jti == refreshJti);


            blockResult.Value.Should().BeEquivalentTo(UserRepositoryResult.Ok);
            blockedRefreshToken.IsBlocked.Should().BeTrue();
        }
    }
}
