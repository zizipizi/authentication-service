using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Controllers;
using Authentication.Tests.UserControllerTests.Utils;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Xunit;

namespace Authentication.Tests.UserControllerTests
{
    public class WhenUserSignOut
    {
        [Fact]
        public async Task SignOut_Ok()
        {
            var userService = FakeUserServiceFactory.UserSignOut(HttpStatusCode.NoContent);

            var tokenModel = FakeModels.FakeTokenModel();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "asd"),
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim(JwtRegisteredClaimNames.Jti, "jti333")
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

            var result = await userController.SignOut(tokenModel, CancellationToken.None);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task SignOut_ServiceUnavailable()
        {
            var tokenModel = FakeModels.FakeTokenModel();

            var userService = FakeUserServiceFactory.UserSignOut(HttpStatusCode.ServiceUnavailable);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "asd"),
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim(JwtRegisteredClaimNames.Jti, "jti333")
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


            var result = await userController.SignOut(tokenModel, CancellationToken.None);

            result.Should().Match<StatusCodeResult>(c => c.StatusCode == StatusCodes.Status503ServiceUnavailable);
        }
    }
}
