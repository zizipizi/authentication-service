using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Models.Domain;
using Authentication.Host.Models;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Moq;

namespace Authentication.Tests.AdminControllerTests.Utills
{
    public static class FakeAdminServiceFactory
    {
        // Create user
        public static IAdminService CreateUser(AdminResult result, string message)
        {
            var userServiceFake = new Mock<IAdminService>();

            userServiceFake.Setup(c => c.CreateUserAsync(It.IsAny<UserCreateModel>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new Result<AdminResult, UserInfo>(result, It.IsAny<UserInfo>(), message)));

            return userServiceFake.Object;
        }

        // Block user
        public static IAdminService BlockUser(AdminResult result, string message)
        {
            var userServiceFake = new Mock<IAdminService>();

            userServiceFake.Setup(c => c.BlockUserAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new Result<AdminResult>(result, message)));

            return userServiceFake.Object;
        }

        // Delete user
        public static IAdminService DeleteUser(AdminResult result, string message)
        {
            var userServiceFake = new Mock<IAdminService>();

            userServiceFake.Setup(c => c.DeleteUserAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new Result<AdminResult>(result, message)));

            return userServiceFake.Object;
        }

    }
}
