using Authentication.Tests.AuthControllerTests.Utils;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Controllers;
using Authentication.Host.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Authentication.Tests.AuthControllerTests
{
    public class WhenSignIn
    {
        [Fact]
        public async Task SignIn_Ok()
        {
            var loginModel = new LoginModel
            {
                UserName = "Terminator",
                Password = "Terminator2013"
            };

            var bodyTokenModel = new BodyTokenModel
            {
                AccessToken = "asdasdasdasd",
                RefreshToken = "asdasddawqe"
            };

            var authService = FakeAuthServiceFactory.FakeSignIn(HttpStatusCode.OK, bodyTokenModel);

            var authController = new AuthController(authService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        Connection =
                        {
                            RemoteIpAddress = new IPAddress(123)
                        }
                    }
                }
            };

            var result = await authController.SignIn(loginModel, CancellationToken.None);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task SignIn_NotFound()
        {
            var authService = FakeAuthServiceFactory.FakeSignIn(HttpStatusCode.NotFound);

            var authController = new AuthController(authService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        Connection =
                        {
                            RemoteIpAddress = new IPAddress(123)
                        }
                    }
                }
            };

            var loginModel = new LoginModel
            {
                UserName = "Terminator",
                Password = "Terminator2013"
            };

            var result = await authController.SignIn(loginModel, CancellationToken.None);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task SignIn_UserBlocked()
        {
            var authService = FakeAuthServiceFactory.FakeSignIn(HttpStatusCode.Unauthorized);


            var authController = new AuthController(authService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        Connection =
                        {
                            RemoteIpAddress = new IPAddress(123)
                        }
                    }
                }
            };
            
            var loginModel = new LoginModel
            {
                UserName = "Terminator",
                Password = "Terminator2013"
            };

            var result = await authController.SignIn(loginModel, CancellationToken.None);

            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task SignIn_ServiceUnavailable()
        {
            var authService = FakeAuthServiceFactory.FakeSignIn(HttpStatusCode.ServiceUnavailable);

            var authController = new AuthController(authService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        Connection =
                        {
                            RemoteIpAddress = new IPAddress(123)
                        }
                    }
                }
            };

            var loginModel = new LoginModel
            {
                UserName = "Terminator",
                Password = "Terminator2013"
            };

            var result = await authController.SignIn(loginModel, CancellationToken.None);

            result.Should().Match<StatusCodeResult>(c => c.StatusCode == StatusCodes.Status503ServiceUnavailable);
        }
    }
}
