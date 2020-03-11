using System.Net;
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
        public static IAdminService CreateUser(HttpStatusCode statusCode, string message = "")
        {
            var userServiceFake = new Mock<IAdminService>();

            userServiceFake.Setup(c => c.CreateUserAsync(It.IsAny<UserCreateModel>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new Result<HttpStatusCode, UserInfo>(statusCode, It.IsAny<UserInfo>(), message)));

            return userServiceFake.Object;
        }

        // Block user
        public static IAdminService BlockUser(HttpStatusCode statusCode, string message = "")
        {
            var userServiceFake = new Mock<IAdminService>();

            userServiceFake.Setup(c => c.BlockUserAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<HttpStatusCode>(statusCode));

            return userServiceFake.Object;
        }

        // Delete user
        public static IAdminService DeleteUser(HttpStatusCode statusCode, string message = "")
        {
            var userServiceFake = new Mock<IAdminService>();

            userServiceFake.Setup(c => c.DeleteUserAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new Result<HttpStatusCode>(statusCode, message)));

            return userServiceFake.Object;
        }

    }
}
