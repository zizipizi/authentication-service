using System;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Models;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Moq;
using NSV.Security.JWT;

namespace Authentication.Tests.UserControllerTests.Utils
{
    public static class FakeUserServiceFactory
    {
        public static IUserService UserSignOut(UserResult result, string message)
        {
            var userServiceFake = new Mock<IUserService>();

            userServiceFake.Setup(c => c.SignOut(It.IsAny<BodyTokenModel>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None))
                .Returns(Task.FromResult(new Result<UserResult>(result)));

            return userServiceFake.Object;
        }

        public static IUserService UserChangePassword(UserResult result, BodyTokenModel model, string message)
        {
            var userServiceFake = new Mock<IUserService>();

            userServiceFake.Setup(c => c.ChangePasswordAsync(It.IsAny<ChangePassModel>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None))
                .Returns(Task.FromResult(new Result<UserResult, TokenModel>(result)));

            return userServiceFake.Object;
        }
    }

    public static class FakeModels
    {
        public static BodyTokenModel FakeTokenModel()
        {
            return new BodyTokenModel
            {
                AccessToken = "asdad",
                RefreshToken = "asdasd"
            };
        }

        public static ChangePassModel FakePasswords()
        {
            return new ChangePassModel
            {
                NewPassword = "Hello",
                OldPassword = "Hello"
            };
        }
    }
}
