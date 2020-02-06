using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Exceptions;
using Authentication.Host.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Authentication.Tests.RepositoryTests
{
    public class WhenGetUserByName
    {
        [Fact]
        public async Task GetUserByName_Ok()
        {
            var authContext = FakeContextFactory.GetUserByName_Ok();
            var logger = new Mock<ILogger<UserRepository>>().Object;

            var tokenRepository = new TokenRepository(authContext, new Mock<ILogger<TokenRepository>>().Object);
            var userRepository = new UserRepository(tokenRepository, authContext, logger);

            var result = await userRepository.GetUserByNameAsync("Login", CancellationToken.None);
            var result2 = await userRepository.GetUserByNameAsync("Login2", CancellationToken.None);


            Assert.Equal("Login", result.Login);
            Assert.Equal("Login2", result2.Login);
        }

        [Fact]
        public async Task GetUserByName_EntityException()
        {
            var authContext = FakeContextFactory.GetUserByName_Ok();
            var logger = new Mock<ILogger<UserRepository>>().Object;

            var tokenRepository = new TokenRepository(authContext, new Mock<ILogger<TokenRepository>>().Object);
            var userRepository = new UserRepository(tokenRepository, authContext, logger);

            var result = userRepository.GetUserByNameAsync("Login4", CancellationToken.None);
            var result2= userRepository.GetUserByNameAsync("Login5", CancellationToken.None);

            var ex = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await result);
            Assert.Equal("User not found", ex.Message);
            var ex2 = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await result2);
            Assert.Equal("User not found", ex.Message);
        }
    }
}
