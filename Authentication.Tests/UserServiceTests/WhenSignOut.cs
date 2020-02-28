using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Models;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using NSV.Security.JWT;
using NSV.Security.Password;
using Xunit;

namespace Authentication.Tests.UserServiceTests
{
    public class WhenSignOut
    {
        [Fact]
        public async Task SignOut_Ok()
        {
            var logger = new Mock<ILogger<UserService>>().Object;
            var passwordService = new Mock<IPasswordService>().Object;
            var jwtService = new Mock<IJwtService>().Object;

            var fakeUserRepository = FakeRepositoryFactory.FakeUser();
            var fakeTokenRepository = FakeRepositoryFactory.BlockRefreshToken_Ok();

            var userService = new UserService(fakeUserRepository, fakeTokenRepository, passwordService, jwtService, logger);

            var result = await userService.SignOutAsync(1, "asdasd", CancellationToken.None);

            Assert.Equal(UserResult.Ok, result.Value);
        }

        [Fact]
        public async Task SignOut_ThrowsException()
        {
            var logger = new Mock<ILogger<UserService>>().Object;
            var passwordService = new Mock<IPasswordService>().Object;
            var jwtService = new Mock<IJwtService>().Object;

            var fakeUserRepository = FakeRepositoryFactory.FakeUser();
            var fakeTokenRepository = FakeRepositoryFactory.BlockRefreshToken_Exception();

            var userService = new UserService(fakeUserRepository, fakeTokenRepository, passwordService, jwtService, logger);

            var result = await userService.SignOutAsync(1, "asdasd", CancellationToken.None);

            Assert.Equal(UserResult.Error, result.Value);
        }

    }
}
