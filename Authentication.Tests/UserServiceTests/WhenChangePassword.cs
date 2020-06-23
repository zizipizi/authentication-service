using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Models;
using Authentication.Host.Repositories;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Authentication.Tests.UserServiceTests.Utils;
using FluentAssertions;
using Moq;
using NSV.Security.JWT;
using NSV.Security.Password;
using Processing.ControlSystem.InternalInteractionModels.InternalAuthEvent;
using Processing.Kafka.Producer;
using Xunit;

namespace Authentication.Tests.UserServiceTests
{
    public class WhenChangePassword
    {
        [Fact]
        public async Task ChangePassword_Ok()
        {
            var passwordService = FakePasswordServiceFactory.FakeValidate(PasswordValidateResult.ValidateResult.Ok);

            var jwtService = FakeJwtServiceFactory.FakeIssueAccessToken(JwtTokenResult.TokenResult.Ok);

            var cacheRepo = new Mock<ICacheRepository>().Object;

            var kafka = new Mock<IProducerFactory<string, BlockedTokenModel>>();
            var producer = new Mock<IKafkaProducer<string, BlockedTokenModel>>();

            kafka.Setup(c => c.GetOrCreate(It.IsAny<string>(), null))
                .Returns(producer.Object);

            var userRepoOptions = new UserRepoOptionsBuilder()
                .GetUserByIdReturns(UserRepositoryResult.Ok)
                .UpdateUserPasswordsReturns(UserRepositoryResult.Ok)
                .BlockAllRefreshTokensReturns(UserRepositoryResult.Ok)
                .Build();

            var userRepo = FakeUserRepositoryFactory.FakeUserRepo(userRepoOptions);

            var userService = new UserService(userRepo, passwordService, jwtService, cacheRepo, kafka.Object);

            var changePassModel = new ChangePassModel
            {
                OldPassword = "asdasd",
                NewPassword = "qweqwe"
            };

            var result = await userService.ChangePasswordAsync(changePassModel, 1, "asdasd", CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ChangePassword_WrongOldPassword()
        {
            var passwordService = FakePasswordServiceFactory.FakeValidate(PasswordValidateResult.ValidateResult.Invalid);

            var jwtService = FakeJwtServiceFactory.FakeIssueAccessToken(JwtTokenResult.TokenResult.Ok);

            var cacheRepo = new Mock<ICacheRepository>().Object;

            var kafka = new Mock<IProducerFactory<string, BlockedTokenModel>>().Object;

            var userRepoOptions = new UserRepoOptionsBuilder()
                .GetUserByIdReturns(UserRepositoryResult.Ok)
                .UpdateUserPasswordsReturns(UserRepositoryResult.Ok)
                .BlockAllRefreshTokensReturns(UserRepositoryResult.Ok)
                .Build();

            var userRepo = FakeUserRepositoryFactory.FakeUserRepo(userRepoOptions);

            var userService = new UserService(userRepo, passwordService, jwtService, cacheRepo, kafka);

            var changePassModel = new ChangePassModel
            {
                OldPassword = "asdasd",
                NewPassword = "qweqwe"
            };

            var result = await userService.ChangePasswordAsync(changePassModel, 1, "asdasd", CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ChangePassword_ServiceUnavailable()
        {
            var passwordService = new Mock<IPasswordService>().Object;
            var jwtService = new Mock<IJwtService>().Object;
            var cacheRepo = new Mock<ICacheRepository>().Object;
            var kafka = new Mock<IProducerFactory<string, BlockedTokenModel>>().Object;

            var userRepoOptions = new UserRepoOptionsBuilder()
                .GetUserByIdReturns(UserRepositoryResult.Ok)
                .UpdateUserPasswordsReturns(UserRepositoryResult.Error)
                .BlockAllRefreshTokensReturns(UserRepositoryResult.Ok)
                .Build();


            var userRepo = FakeUserRepositoryFactory.FakeUserRepo(userRepoOptions);

            var userService = new UserService(userRepo, passwordService, jwtService, cacheRepo, kafka);

            var changePassModel = new ChangePassModel
            {
                OldPassword = "asdasd",
                NewPassword = "qweqwe"
            };

            var result = await userService.ChangePasswordAsync(changePassModel, 1, "asdasd", CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.ServiceUnavailable);
        }

    }
}
