using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NSV.Security.JWT;
using NSV.Security.Password;
using Xunit;

namespace Authentication.Tests.AdminServiceTests
{
    public class WhenBlockUser
    {
        [Fact]
        public async Task BlockUser_Success()
        {
            var id = 1;
            var passwordService = new Mock<IPasswordService>().Object;
            var logger = new Mock<ILogger<AdminService>>().Object;

            var userRepo = FakeRepositoryFactory.BlockFakeUser();
            var userService = new AdminService(userRepo, passwordService, logger);

            var result = await userService.BlockUserAsync(id, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(AdminResult.Ok);
            //Assert.Equal(AdminResult.Ok, result.Value);
        }

        [Fact]
        public async Task BlockUser_NotFound()
        {
            var id = 1;
            var passwordService = new Mock<IPasswordService>().Object;
            var logger = new Mock<ILogger<AdminService>>().Object;


            var userRepo = FakeRepositoryFactory.BlockFakeUser_Exception();
            var userService = new AdminService(userRepo, passwordService, logger);

            var result = await userService.BlockUserAsync(id, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(AdminResult.UserNotFound);
            //Assert.Equal(AdminResult.UserNotFound, result.Value);
        }
    }
}
