using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Authentication.Tests.UserServiceTests.Utils;
using FluentAssertions;
using Moq;
using NSV.Security.JWT;
using NSV.Security.Password;
using Processing.Kafka.Producer;
using Xunit;

namespace Authentication.Tests.UserServiceTests
{
    public class WhenSignOut
    {
        [Fact]
        public async Task SignOut_Ok()
        {
            var passwordService = FakePasswordServiceFactory.FakeValidate(PasswordValidateResult.ValidateResult.Invalid);

            var jwtService = FakeJwtServiceFactory.FakeIssueAccessToken(JwtTokenResult.TokenResult.Ok);

            var cacheRepo = FakeCacheRepositoryFactory.FakeCacheRepository(CacheRepositoryResult.IsNotBlocked, CacheRepositoryResult.Ok);

            var kafka = new Mock<IProducerFactory<long, string>>();
            var producer = new Mock<IKafkaProducer<long, string>>();

            kafka.Setup(c => c.GetOrCreate(It.IsAny<string>(), null))
                .Returns(producer.Object);

            var userRepoOptions = new UserRepoOptionsBuilder()
                .GetUserByIdReturns(UserRepositoryResult.Ok)
                .UpdateUserPasswordsReturns(UserRepositoryResult.Ok)
                .BlockAllRefreshTokensReturns(UserRepositoryResult.Ok)
                .BlockRefreshTokenReturns(UserRepositoryResult.Ok)
                .Build();

            var userRepo = FakeUserRepositoryFactory.FakeUserRepo(userRepoOptions);

            var userService = new UserService(userRepo, passwordService, jwtService, cacheRepo, kafka.Object);

            var result = await userService.SignOutAsync(1, "asdasd", CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task SignOut_BadRequest()
        {
            var passwordService = FakePasswordServiceFactory.FakeValidate(PasswordValidateResult.ValidateResult.Invalid);

            var jwtService = FakeJwtServiceFactory.FakeIssueAccessToken(JwtTokenResult.TokenResult.Ok);

            var cacheRepo = FakeCacheRepositoryFactory.FakeCacheRepository(CacheRepositoryResult.IsNotBlocked, CacheRepositoryResult.Ok);

            var kafka = new Mock<IProducerFactory<long, string>>().Object;

            var userRepoOptions = new UserRepoOptionsBuilder()
                .GetUserByIdReturns(UserRepositoryResult.Ok)
                .UpdateUserPasswordsReturns(UserRepositoryResult.Ok)
                .BlockAllRefreshTokensReturns(UserRepositoryResult.Ok)
                .BlockRefreshTokenReturns(UserRepositoryResult.Error)
                .Build();

            var userRepo = FakeUserRepositoryFactory.FakeUserRepo(userRepoOptions);

            var userService = new UserService(userRepo, passwordService, jwtService, cacheRepo, kafka);

            var result = await userService.SignOutAsync(1, "asdasd", CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.BadRequest);
        }

    }
}
