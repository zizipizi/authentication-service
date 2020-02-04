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
using Xunit;

namespace Authentication.Tests.RepositoryTests
{
    public class WhenChangePassword
    {
        [Fact]
        public async Task ChangePassword_Ok()
        {
            var authContext = FakeContextFactory.UpdateUserPassword_Ok();
            var logger = new Mock<ILogger<UserRepository>>().Object;

            var userRepository = new UserRepository(authContext, logger);

            await userRepository.UpdateUserPassword(1, "NewPassword", CancellationToken.None);

            var user = authContext.Users.FirstOrDefault(c => c.Id == 1);

            Assert.Equal("NewPassword", user.Password);
        }

        [Fact]
        public async Task ChangePassword_EntityException()
        {
            var authContext = FakeContextFactory.UpdateUserPassword_EntityException();
            var logger = new Mock<ILogger<UserRepository>>().Object;

            var userRepository = new UserRepository(authContext, logger);

            var ex = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await userRepository.UpdateUserPassword(2, "NewPassword", CancellationToken.None));
            Assert.Equal("User not found", ex.Message);
        }
    }
}
