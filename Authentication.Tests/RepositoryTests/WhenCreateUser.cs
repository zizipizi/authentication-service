using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Exceptions;
using Authentication.Data.Models.Domain;
using Authentication.Host.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Authentication.Tests.RepositoryTests
{
    public class WhenCreateUser
    {
        [Fact]
        public async Task CreateUser_Ok()
        {
            var user = new User 
            {
                Login = "Login",
                Password = "Password",
                Id = 1,
                IsActive = true,
                UserName = "UserName",
                Role = new List<string> { "Admin", "User" }
            };

            var authContext = FakeContextFactory.CreateUser_Ok();
            var logger = new Mock<ILogger<UserRepository>>().Object;
            var cache = new Mock<IDistributedCache>().Object;

            var tokenRepository = new TokenRepository(authContext, new Mock<ILogger<TokenRepository>>().Object, cache);
            var userRepository = new UserRepository(tokenRepository, authContext, logger);

            await userRepository.CreateUserAsync(user, CancellationToken.None);

            var result = authContext.Users.FirstOrDefault(c => c.Login == "Login");

            Assert.Equal("Login", result.Login);
            Assert.Equal("Password", result.Password);
        }

        [Fact]
        public async Task CreateUser_EntityException()
        {
            var user = new User
            {
                Login = "Login",
                Password = "Password",
                Id = 1,
                IsActive = true,
                UserName = "UserName",
                Role = new List<string> { "Admin", "User" }
            };

            var newUser = new User
            {
                Login = "Login",
                Password = "Password",
                Id = 1,
                IsActive = true,
                UserName = "UserName",
                Role = new List<string> { "Admin", "User" }
            };

            var authContext = FakeContextFactory.CreateUser_EntityException();
            var logger = new Mock<ILogger<UserRepository>>().Object;
            var cache = new Mock<IDistributedCache>().Object;

            var tokenRepository = new TokenRepository(authContext, new Mock<ILogger<TokenRepository>>().Object, cache);
            var userRepository = new UserRepository(tokenRepository, authContext, logger);

            await userRepository.CreateUserAsync(user, CancellationToken.None);

            var ex = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await userRepository.CreateUserAsync(newUser, CancellationToken.None));
            Assert.Equal("User already exist", ex.Message);
        }

    }
}
