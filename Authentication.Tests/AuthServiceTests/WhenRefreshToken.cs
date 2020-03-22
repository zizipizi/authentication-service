using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Models;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Authentication.Tests.AuthServiceTests.Utils;
using FluentAssertions;
using Moq;
using NSV.Security.JWT;
using NSV.Security.Password;
using Xunit;

namespace Authentication.Tests.AuthServiceTests
{
    public class WhenRefreshToken
    {
        [Fact]
        public async Task RefreshToken_OK()
        {
            var passwordService = new Mock<IPasswordService>().Object;

            var jwtService = FakeJwtServiceFactory.FakeRefreshAccessToken(JwtTokenResult.TokenResult.Ok);

            var cacheRepo = FakeCacheRepositoryFactory.FakeIsRefreshTokenBlockedAsync(CacheRepositoryResult.IsNotBlocked);

            var authRepoOptions = new AuthRepoOptionsBuilder()
                .UserIsActive(true)
                .AddTokensMethodReturns(AuthRepositoryResult.Ok)
                .GetUserByNameReturns(AuthRepositoryResult.Ok)
                .Build();

            var authRepo = FakeAuthRepositoryFactory.FakeAuthRepository(authRepoOptions);

            var authService = new AuthService(jwtService, passwordService, cacheRepo, authRepo);

            var bodyTokenModel = new BodyTokenModel
            {
                AccessToken = "adasdasd",
                RefreshToken = "asdasdfsdf"
            };

            var result = await authService.RefreshToken(bodyTokenModel, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.OK);
        }

        [Fact]
        public async Task RefreshToken_TokenIsBlocked()
        {

            var passwordService = new Mock<IPasswordService>().Object;

            var jwtService = FakeJwtServiceFactory.FakeRefreshAccessToken(JwtTokenResult.TokenResult.Ok);

            var cacheRepo = FakeCacheRepositoryFactory.FakeIsRefreshTokenBlockedAsync(CacheRepositoryResult.IsBlocked);

            var authRepoOptions = new AuthRepoOptionsBuilder()
                .UserIsActive(true)
                .AddTokensMethodReturns(AuthRepositoryResult.Ok)
                .GetUserByNameReturns(AuthRepositoryResult.Ok)
                .Build();

            var authRepo = FakeAuthRepositoryFactory.FakeAuthRepository(authRepoOptions);

            var authService = new AuthService(jwtService, passwordService, cacheRepo, authRepo);

            var bodyTokenModel = new BodyTokenModel
            {
                AccessToken = "adasdasd",
                RefreshToken = "asdasdfsdf"
            };

            var result = await authService.RefreshToken(bodyTokenModel, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task RefreshToken_ServiceUnavailable()
        {
            var passwordService = new Mock<IPasswordService>().Object;

            var jwtService = FakeJwtServiceFactory.FakeRefreshAccessToken(JwtTokenResult.TokenResult.Ok);

            var cacheRepo = FakeCacheRepositoryFactory.FakeIsRefreshTokenBlockedAsync(CacheRepositoryResult.Ok);

            var authRepoOptions = new AuthRepoOptionsBuilder()
                .UserIsActive(true)
                .AddTokensMethodReturns(AuthRepositoryResult.Error)
                .GetUserByNameReturns(AuthRepositoryResult.Ok)
                .Build();

            var authRepo = FakeAuthRepositoryFactory.FakeAuthRepository(authRepoOptions);

            var authService = new AuthService(jwtService, passwordService, cacheRepo, authRepo);

            var bodyTokenModel = new BodyTokenModel
            {
                AccessToken = "adasdasd",
                RefreshToken = "asdasdfsdf"
            };

            var result = await authService.RefreshToken(bodyTokenModel, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.ServiceUnavailable);
        }

        [Fact]
        public async Task RefrehToken_TokenNotValidated()
        {

            var passwordService = new Mock<IPasswordService>().Object;

            var jwtService = FakeJwtServiceFactory.FakeRefreshAccessToken(JwtTokenResult.TokenResult.RefreshTokenInvalid);

            var cacheRepo = FakeCacheRepositoryFactory.FakeIsRefreshTokenBlockedAsync(CacheRepositoryResult.Ok);

            var authRepoOptions = new AuthRepoOptionsBuilder()
                .UserIsActive(true)
                .AddTokensMethodReturns(AuthRepositoryResult.Error)
                .GetUserByNameReturns(AuthRepositoryResult.Ok)
                .Build();

            var authRepo = FakeAuthRepositoryFactory.FakeAuthRepository(authRepoOptions);

            var authService = new AuthService(jwtService, passwordService, cacheRepo, authRepo);

            var bodyTokenModel = new BodyTokenModel
            {
                AccessToken = "adasdasd",
                RefreshToken = "asdasdfsdf"
            };

            var result = await authService.RefreshToken(bodyTokenModel, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }
    }
}
