using System;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Models;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using NSV.Security.JWT;
using NSV.Security.Password;
using Xunit;

namespace Authentication.Tests.AuthServiceTests
{
    public class WhenRefreshToken
    {
        [Fact]
        public async Task RefrehToken_OK()
        {
            var logger = new Mock<ILogger<AuthService>>().Object;
            var passwordService = new Mock<IPasswordService>().Object;
            var jwtService = new Mock<IJwtService>();
            jwtService
                .Setup(x => x.RefreshAccessToken(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string a, string r) => new JwtTokenResult(JwtTokenResult.TokenResult.Ok,
                    new TokenModel((a, DateTime.MaxValue, Guid.NewGuid().ToString()),
                        (r, DateTime.MaxValue, Guid.NewGuid().ToString())), "1"));

            var cache = new Mock<IDistributedCache>();
            cache.Setup(x => x.GetAsync(It.IsAny<string>(), CancellationToken.None))
                .Returns(Task.FromResult((byte[]) null));

            var fakeTokenRepository = FakeRepositoryFactory.IsRefreshTokenBlocked_Ok();
            var fakeUserRepository = FakeRepositoryFactory.FakeUser();

            var authService = new AuthService(jwtService.Object, passwordService, fakeUserRepository, fakeTokenRepository, logger, cache.Object);

            var bodyTokenModel = new BodyTokenModel
            {
                AccessToken = "adasdasd",
                RefreshToken = "asdasdfsdf"
            };

            var result = await authService.RefreshToken(bodyTokenModel, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(AuthResult.Ok);
            //Assert.Equal(AuthResult.Ok, result.Value);
        }

        [Fact]
        public async Task RefrehToken_Blocked()
        {
            var logger = new Mock<ILogger<AuthService>>().Object;
            var passwordService = new Mock<IPasswordService>().Object;
            var jwtService = new Mock<IJwtService>();
            var cache = new Mock<IDistributedCache>();

            cache.Setup(x => x.GetAsync(It.IsAny<string>(), CancellationToken.None))
                .Returns(Task.FromResult((new byte['2'])));

            jwtService
                .Setup(x => x.RefreshAccessToken(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string a, string r) => new JwtTokenResult(JwtTokenResult.TokenResult.Ok,
                    new TokenModel((a, DateTime.MaxValue, Guid.NewGuid().ToString()),
                        (r, DateTime.MaxValue, Guid.NewGuid().ToString())), "1"));

            var fakeTokenRepository = FakeRepositoryFactory.IsRefreshTokenBlocked_TokenBlocked();
            var fakeUserRepository = FakeRepositoryFactory.FakeUser();

            var authService = new AuthService(jwtService.Object, passwordService, fakeUserRepository, fakeTokenRepository, logger, cache.Object);

            var bodyTokenModel = new BodyTokenModel
            {
                AccessToken = "adasdasd",
                RefreshToken = "asdasdfsdf"
            };

            var result = await authService.RefreshToken(bodyTokenModel, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(AuthResult.TokenIsBlocked);
            //Assert.Equal(AuthResult.TokenIsBlocked, result.Value);
        }
    }
}
