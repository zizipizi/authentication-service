using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Controllers;
using Authentication.Host.Results.Enums;
using Authentication.Tests.UserControllerTests.Utils;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Authentication.Tests.UserControllerTests
{
    public class WhenUserChangePassword
    {
        [Fact]
        public async Task UserChangePassword_Ok()
        {
            var tokenModel = FakeModels.FakeTokenModel();
            var changePassModel = FakeModels.FakePasswords();

            var userService = FakeUserServiceFactory.UserChangePassword( HttpStatusCode.OK, tokenModel, "");

            var user = new ClaimsPrincipal(new ClaimsIdentity(new []
            {
                new Claim(ClaimTypes.Name, "asd"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, "User"),
            }
            ));

            var userController = new UserController(userService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = user,
                        Request = { Headers = { ["Authorization"] = "asdk" } }
                    }
                }
            };

            var result = await userController.ChangePassword(changePassModel, CancellationToken.None);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task UserChangePassword_WrongOldPassword()
        {
            var tokenModel = FakeModels.FakeTokenModel();
                var changePassModel = FakeModels.FakePasswords();

                var userService = FakeUserServiceFactory.UserChangePassword(HttpStatusCode.BadRequest, tokenModel, "");
                var user = new ClaimsPrincipal(new ClaimsIdentity(new []
                    {
                        new Claim(ClaimTypes.Name, "asd"),
                        new Claim(ClaimTypes.NameIdentifier, "1"),
                        new Claim(ClaimTypes.Role, "User"),
                    }
                ));

            var userController = new UserController(userService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = user,
                        Request = { Headers = { ["Authorization"] = "asdk" } }
                    }
                }
            };

            var result = await userController.ChangePassword(changePassModel, CancellationToken.None);

                result.Should().BeOfType<BadRequestObjectResult>();
            }

        [Fact]
        public async Task UserChangePassword_ServiceUnavailable()
        {
            var tokenModel = FakeModels.FakeTokenModel();
            var changePassModel = FakeModels.FakePasswords();

            var logger = new Mock<ILogger<UserController>>().Object;

            var userService = FakeUserServiceFactory.UserChangePassword(HttpStatusCode.ServiceUnavailable, tokenModel);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new []
                {
                    new Claim(ClaimTypes.Name, "asd"),
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim(ClaimTypes.Role, "User"),
                }
            ));

            var userController = new UserController(userService, logger)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = user,
                        Request = { Headers = { ["Authorization"] = "asdk" } }
                    }
                }
            };

            var result = await userController.ChangePassword(changePassModel, CancellationToken.None);

            result.Should().BeOfType<StatusCodeResult>();
            //Assert.IsType<NoContentResult>(result);
        }

    }
}
