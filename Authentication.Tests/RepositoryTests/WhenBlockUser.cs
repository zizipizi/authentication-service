using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Exceptions;
using Authentication.Data.Models.Domain;
using Authentication.Host.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Authentication.Tests.RepositoryTests
{
    public class WhenBlockUser
    {
        [Fact]
        public async Task BlockUser_Ok()
        {
            var authContext = FakeContextFactory.BlockUser_Ok();
            var cache = new Mock<IDistributedCache>().Object;

            var tokenRepository = new TokenRepository(authContext, new Mock<ILogger<TokenRepository>>().Object, cache);
            var userRepository = new UserRepository(tokenRepository, authContext, new Mock<ILogger<UserRepository>>().Object);

            await userRepository.BlockUserAsync(1, CancellationToken.None);

            var result = await authContext.Users.FirstOrDefaultAsync(c => c.Id == 1);

            Assert.False(result.IsActive);
        }

        [Fact]
        public async Task BlockUser_EntityException()
        {
            var authContext = FakeContextFactory.BlockUser_EntityException();
            var logger = new Mock<ILogger<UserRepository>>().Object;
            var cache = new Mock<IDistributedCache>().Object;

            var tokenRepository = new TokenRepository(authContext, new Mock<ILogger<TokenRepository>>().Object, cache);
            var userRepository = new UserRepository(tokenRepository, authContext, logger);

            var ex = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await userRepository.BlockUserAsync(2, CancellationToken.None));
            Assert.Equal("User not found", ex.Message);

        }
    }
}
