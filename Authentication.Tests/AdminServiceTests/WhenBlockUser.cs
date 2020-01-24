using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Moq;
using NSV.Security.JWT;
using NSV.Security.Password;
using Xunit;

namespace Authentication.Tests.AdminServiceTests
{
    public class WhenBlockUser
    {
        [Fact]
        public async Task BlockUser_Sucess()
        {
            var id = 1;
            var passwordService = new Mock<IPasswordService>();

            var userRepo = FakeRepositoryFactory.BlockFakeUserRepository();
            var userService = new AdminService(userRepo, passwordService.Object);

            var result = await userService.BlockUserAsync(id, CancellationToken.None);

            Assert.IsType<Result<AdminResult>>(result);
            Assert.Equal(AdminResult.Ok, result.Value);
        }

        [Fact]
        public async Task BlockUser_NotFound()
        {
            var id = 1;
            var passwordService = new Mock<IPasswordService>();

            var userRepo = FakeRepositoryFactory.BlockFakeUserRepository_Exception();
            var userService = new AdminService(userRepo, passwordService.Object);

            var result = await userService.BlockUserAsync(id, CancellationToken.None);

            Assert.IsType<Result<AdminResult>>(result);
            Assert.Equal(AdminResult.UserNotFound, result.Value);
        }
    }
}
