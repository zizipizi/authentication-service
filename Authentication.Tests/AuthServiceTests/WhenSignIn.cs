using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Models;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Authentication.Tests.AuthServiceTests.Utils;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using NSV.Security.JWT;
using NSV.Security.Password;
using Xunit;

namespace Authentication.Tests.AuthServiceTests
{
    public class WhenSignIn
    {
        [Fact]
        public async Task SignIn_Ok()
        {
            var passwordService = FakePasswordServiceFactory.FakeValidate(PasswordValidateResult.ValidateResult.Ok);

            var jwtService = FakeJwtServiceFactory.FakeIssueAccessToken(JwtTokenResult.TokenResult.Ok);

            var cacheRepo = FakeCacheRepositoryFactory.FakeIsRefreshTokenBlockedAsync(CacheRepositoryResult.Ok);

            var authRepoOptions = new AuthRepoOptionsBuilder()
                .UserIsActive(true)
                .AddTokensMethodReturns(AuthRepositoryResult.Ok)
                .GetUserByNameReturns(AuthRepositoryResult.Ok)
                .Build();

            var authRepo = FakeAuthRepositoryFactory.FakeAuthRepository(authRepoOptions);

            var authService = new AuthService(jwtService, passwordService, cacheRepo, authRepo);

            var loginModel = new LoginModel
            {
                UserName = "Login",
                Password = "Password"
            };

            var result = await authService.SignIn(loginModel, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.OK);
        }

        [Fact]
        public async Task SignIn_WrongLoginOrPassword()
        {
            var passwordService = FakePasswordServiceFactory.FakeValidate(PasswordValidateResult.ValidateResult.Invalid);

            var jwtService = new Mock<IJwtService>().Object;

            var cacheRepo = FakeCacheRepositoryFactory.FakeIsRefreshTokenBlockedAsync(CacheRepositoryResult.IsNotBlocked);

            var authRepoOptions = new AuthRepoOptionsBuilder()
                .UserIsActive(true)
                .AddTokensMethodReturns(AuthRepositoryResult.Ok)
                .GetUserByNameReturns(AuthRepositoryResult.Ok)
                .Build();

            var authRepo = FakeAuthRepositoryFactory.FakeAuthRepository(authRepoOptions);

            var authService = new AuthService(jwtService, passwordService, cacheRepo, authRepo);

            var loginModel = new LoginModel
            {
                UserName = "UserName",
                Password = "Password2020"
            };

            var result = await authService.SignIn(loginModel, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task SignIn_UserIsBlocked()
        {
            var passwordService = FakePasswordServiceFactory.FakeValidate(PasswordValidateResult.ValidateResult.Invalid);

            var jwtService = new Mock<IJwtService>().Object;

            var cacheRepo = FakeCacheRepositoryFactory.FakeIsRefreshTokenBlockedAsync(CacheRepositoryResult.Ok);

            var authRepoOptions = new AuthRepoOptionsBuilder()
                .UserIsActive(false)
                .AddTokensMethodReturns(AuthRepositoryResult.Ok)
                .GetUserByNameReturns(AuthRepositoryResult.Ok)
                .Build();

            var authRepo = FakeAuthRepositoryFactory.FakeAuthRepository(authRepoOptions);

            var authService = new AuthService(jwtService, passwordService, cacheRepo, authRepo);

            var loginModel = new LoginModel
            {
                UserName = "UserName",
                Password = "Password2020"
            };

            var result = await authService.SignIn(loginModel, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task SignIn_ServiceUnavailable()
        {
            var passwordService = FakePasswordServiceFactory.FakeValidate(PasswordValidateResult.ValidateResult.Invalid);

            var jwtService = new Mock<IJwtService>().Object;

            var cacheRepo = FakeCacheRepositoryFactory.FakeIsRefreshTokenBlockedAsync(CacheRepositoryResult.Ok);

            var authRepoOptions = new AuthRepoOptionsBuilder()
                .UserIsActive(false)
                .AddTokensMethodReturns(AuthRepositoryResult.Ok)
                .GetUserByNameReturns(AuthRepositoryResult.Error)
                .Build();

            var authRepo = FakeAuthRepositoryFactory.FakeAuthRepository(authRepoOptions);

            var authService = new AuthService(jwtService, passwordService, cacheRepo, authRepo);

            var loginModel = new LoginModel
            {
                UserName = "UserName",
                Password = "Password2020"
            };

            var result = await authService.SignIn(loginModel, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.ServiceUnavailable);
        }
    }
}
