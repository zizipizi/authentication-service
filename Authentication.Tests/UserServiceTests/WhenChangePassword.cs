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
    public class WhenChangePassword
    {
        [Fact]
        public async Task ChangePassword_Ok()
        {
            var logger = new Mock<ILogger<UserService>>().Object;
            var passwordService = new Mock<IPasswordService>().Object;
            var jwtService = new Mock<IJwtService>().Object;
            var cache = new Mock<IDistributedCache>().Object;

            var fakeUserRepository = FakeRepositoryFactory.ChangePassword_Ok();
            var fakeTokenRepository = FakeRepositoryFactory.BlockAllTokens_Ok();

            var userService = new UserService(fakeUserRepository, fakeTokenRepository, passwordService, jwtService, logger, cache);

            var changePassModel = new ChangePassModel
            {
                OldPassword = "asdasd",
                NewPassword = "qweqwe"
            };

            var result = await userService.ChangePasswordAsync(changePassModel, 1, "asdasd", CancellationToken.None);

            Assert.Equal(UserResult.Ok, result.Value);
        }

        [Fact]
        public async Task ChangePassword_ThrowsEntityException()
        {
            var logger = new Mock<ILogger<UserService>>().Object;
            var passwordService = new Mock<IPasswordService>().Object;
            var jwtService = new Mock<IJwtService>().Object;
            var cache = new Mock<IDistributedCache>().Object;


            var fakeUserRepository = FakeRepositoryFactory.ChangePassword_EntityException();
            var fakeTokenRepository = FakeRepositoryFactory.FakeToken();

            var userService = new UserService(fakeUserRepository, fakeTokenRepository, passwordService, jwtService, logger, cache);

            var changePassModel = new ChangePassModel
            {
                OldPassword = "asdasd",
                NewPassword = "qweqwe"
            };

            var result = await userService.ChangePasswordAsync(changePassModel, 1, "asdasd", CancellationToken.None);

            Assert.Equal(UserResult.Error, result.Value);
        }

        [Fact]
        public async Task ChangePassword_ThrowsException()
        {
            var logger = new Mock<ILogger<UserService>>().Object;
            var passwordService = new Mock<IPasswordService>().Object;
            var jwtService = new Mock<IJwtService>().Object;
            var cache = new Mock<IDistributedCache>().Object;

            var fakeUserRepository = FakeRepositoryFactory.ChangePassword_Exception();
            var fakeTokenRepository = FakeRepositoryFactory.FakeToken();

            var userService = new UserService(fakeUserRepository, fakeTokenRepository, passwordService, jwtService, logger, cache);

            var changePassModel = new ChangePassModel
            {
                OldPassword = "asdasd",
                NewPassword = "qweqwe"
            };

            var result = await userService.ChangePasswordAsync(changePassModel, 1, "asdasd", CancellationToken.None);

            Assert.Equal(UserResult.Error, result.Value);
        }

    }
}
