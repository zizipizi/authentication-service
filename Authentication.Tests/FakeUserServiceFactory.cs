using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Enums;
using Authentication.Host.Models;
using Authentication.Host.Services;
using Moq;

namespace Authentication.Tests
{
    public static class FakeUserServiceFactory
    {
        public static IAdminService CreateFakeUserService(AdminResult result)
        {
            var userServiceFake = new Mock<IAdminService>();

            userServiceFake.Setup(c => c.CreateUserAsync(It.IsAny<UserCreateModel>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new Result<AdminResult>(result)));

            return userServiceFake.Object;
        }

        public static IAdminService GetFakeBlockUserService(AdminResult result)
        {
            var userServiceFake = new Mock<IAdminService>();

            userServiceFake.Setup(c => c.BlockUserAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new Result<AdminResult>(result)));

            return userServiceFake.Object;
        }

        public static IAdminService GetFakeDeleteUserService(AdminResult result)
        {
            var userServiceFake = new Mock<IAdminService>();

            userServiceFake.Setup(c => c.DeleteUserAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new Result<AdminResult>(result)));

            return userServiceFake.Object;
        }
    }
}
