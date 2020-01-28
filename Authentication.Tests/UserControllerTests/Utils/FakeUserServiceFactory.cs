using System;
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

            userServiceFake.Setup(c => c.SignOut(It.IsAny<TokenModel>()))
                .Returns(Task.FromResult(new Result<UserResult>(result)));

            return userServiceFake.Object;
        }

        public static IUserService UserChangePassword(UserResult result, TokenModel model, string message)
        {
            var userServiceFake = new Mock<IUserService>();

            userServiceFake.Setup(c => c.ChangePassword(It.IsAny<ChangePassModel>()))
                .Returns(Task.FromResult(new Result<UserResult, TokenModel>(result)));

            return userServiceFake.Object;
        }
    }

    public static class FakeModels
    {
        public static TokenModel FakeTokenModel()
        {
            return new TokenModel(
                ("asd", DateTime.Now), 
                ("sd", DateTime.Now)
                );
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
