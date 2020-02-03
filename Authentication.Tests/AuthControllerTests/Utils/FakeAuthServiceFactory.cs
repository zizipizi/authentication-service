using System;
using System.Collections.Generic;
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
        public static IAuthService FakeRefreshToken(AuthResult result)
        {
            var authService = new Mock<IAuthService>();

            authService.Setup(c => c.RefreshToken(It.IsAny<BodyTokenModel>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new Result<AuthResult, TokenModel>(result)));

            return authService.Object;
        }


        public static IAuthService FakeSignIn(AuthResult result)
        {
            var authService = new Mock<IAuthService>();

            authService.Setup(c => c.SignIn(It.IsAny<LoginModel>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new Result<AuthResult, TokenModel>(result)));

            return authService.Object;
        }
    }
}
