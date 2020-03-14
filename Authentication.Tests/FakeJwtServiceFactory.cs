using System;
using System.Collections.Generic;
using Moq;
using NSV.Security.JWT;

namespace Authentication.Tests
{
    public static class FakeJwtServiceFactory
    {
        public static IJwtService FakeRefreshAccessToken(JwtTokenResult.TokenResult jwtResult)
        {
            var jwtService = new Mock<IJwtService>();

            var tokensModel = new TokenModel(
                ("AccessToken", DateTime.Now, "accessJti"),
                ("RefreshToken", DateTime.UtcNow, "RefreshJti"));

            jwtService.Setup(c => c.RefreshAccessToken(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new JwtTokenResult(jwtResult, tokensModel, "1"));

            return jwtService.Object;
        }

        public static IJwtService FakeIssueAccessToken(JwtTokenResult.TokenResult jwtResult) 
        {
            var jwtService = new Mock<IJwtService>();

            var tokensModel = new TokenModel(
                ("AccessToken", DateTime.Now, "accessJti"),
                ("RefreshToken", DateTime.UtcNow, "RefreshJti"));


            jwtService.Setup(c => c.IssueAccessToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                .Returns(new JwtTokenResult(jwtResult, tokensModel));

            return jwtService.Object;
        }
    }
}
