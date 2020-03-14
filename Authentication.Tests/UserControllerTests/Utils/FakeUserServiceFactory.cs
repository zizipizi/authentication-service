using System.Net;
using System.Threading;
using Authentication.Host.Models;
using Authentication.Host.Results;
using Authentication.Host.Services;
using Moq;

namespace Authentication.Tests.UserControllerTests.Utils
{
    public static class FakeUserServiceFactory
    {
        public static IUserService UserSignOut(HttpStatusCode statusCode)
        {
            var userServiceFake = new Mock<IUserService>();

            userServiceFake.Setup(c => c.SignOutAsync(It.IsAny<long>(), It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(new Result<HttpStatusCode>(statusCode));

            return userServiceFake.Object;
        }

        public static IUserService UserChangePassword(HttpStatusCode statusCode, BodyTokenModel model = null, string message = "")
        {
            var userServiceFake = new Mock<IUserService>();

            userServiceFake.Setup(c => c.ChangePasswordAsync(It.IsAny<ChangePassModel>(), It.IsAny<long>(), It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(new Result<HttpStatusCode, BodyTokenModel>(statusCode, model));

            return userServiceFake.Object;
        }
    }

    public static class FakeModels
    {
        public static BodyTokenModel FakeTokenModel()
        {
            return new BodyTokenModel
            {
                AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhc2Rhc2QiLCJqdGkiOiJhMjZmNDg0Ni1lYjk2LTQ3MmEtOWI2ZC03NTVmYzZjN2Y1YTYiLCJpYXQiOiIzMC4wMS4yMDIwIDk6MTE6NTYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYXNkYXNkIiwibmJmIjoxNTgwMzc1NTE2LCJleHAiOjE1ODA4MDc1MTYsImlzcyI6ImlkZW50aXR5Lm5zdi5wdWIiLCJhdWQiOiJpZGVudGl0eS5uc3YucHViIn0.lMCevKafs3NjWkj9IbklKUGUT7xqFIB1GDu6w3TX70s",
                RefreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI0IiwianRpIjoiMWZmNTE3ZDUtNDE2YS00OGMwLTlmZGYtYmQ2YjgyZGYxOWQxIiwiaWF0IjoiMDguMDIuMjAyMCA2OjUwOjM3IiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IkRpbW9uMyIsIm5iZiI6MTU4MTE0NDYzNywiZXhwIjoxNTgxNTc2NjM3LCJpc3MiOiJpZGVudGl0eS5uc3YucHViIiwiYXVkIjoiaWRlbnRpdHkubnN2LnB1YiJ9.3yyc1zF7t4cU-LNzIprJn1ZRvXByqB5EIONqhYUDgLY"
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
