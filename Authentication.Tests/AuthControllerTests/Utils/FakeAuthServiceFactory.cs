using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Models;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Moq;
using NSV.Security.JWT;

namespace Authentication.Tests.AuthControllerTests.Utils
{
    public static class FakeAuthServiceFactory
    {
        public static IAuthService FakeRefreshToken(HttpStatusCode statusCode, BodyTokenModel model = null)
        {
            var authService = new Mock<IAuthService>();

            authService.Setup(c => c.RefreshToken(It.IsAny<BodyTokenModel>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new Result<HttpStatusCode, BodyTokenModel>(statusCode, model)));

            return authService.Object;
        }


        public static IAuthService FakeSignIn(HttpStatusCode statusCode, BodyTokenModel model = null)
        {
            var authService = new Mock<IAuthService>();

            authService.Setup(c => c.SignIn(It.IsAny<LoginModel>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new Result<HttpStatusCode, BodyTokenModel>(statusCode, model)));

            return authService.Object;
        }
    }
}
