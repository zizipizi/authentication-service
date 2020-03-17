using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Models.Domain;
using Authentication.Host.Models;
using Authentication.Host.Repositories;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Authentication.Tests.AdminServiceTests.Utils;
using FluentAssertions;
using Moq;
using NSV.Security.Password;
using Processing.ControlSystem.InternalInteractionModels.InternalAuthEvent;
using Processing.Kafka.Producer;
using Xunit;

namespace Authentication.Tests.AdminServiceTests
{
    public class WhenCreateUser
    {
        [Fact]
        public async Task CreateUser_Success()
        {
            var userCreateModel = new UserCreateModel
            {
                Login = "SomeLogin",
                Password = "SomeLongPassword",
                Role = "Admin",
                UserName = "UserName"
            };

            var user = new User
            {
                Login = "SomeLogin",
                Password = "SomeLongPassword",
                Role = userCreateModel.Role.Split(",").Select(p => p.Trim()),
                UserName = "UserName"
            };

            var cacheRepo = new Mock<ICacheRepository>().Object;

            var kafka = new Mock<IProducerFactory<string, BlockedTokenModel>>();
            var producer = new Mock<IKafkaProducer<string, BlockedTokenModel>>();

            kafka.Setup(c => c.GetOrCreate(It.IsAny<string>(), null))
                .Returns(producer.Object);

            var passService = FakePasswordServiceFactory.FakeHashPassword(PasswordHashResult.HashResult.Ok);
            var adminRepo = FakeAdminRepositoryFactory.FakeCreateUser(AdminRepositoryResult.Ok, user);

            var adminService = new AdminService(adminRepo, cacheRepo, passService, kafka.Object);

            var result = await adminService.CreateUserAsync(userCreateModel, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.OK);
        }

        [Fact]
        public async Task CreateUser_Exist()
        {
            var cacheRepo = new Mock<ICacheRepository>().Object;

            var passService = FakePasswordServiceFactory.FakeHashPassword(PasswordHashResult.HashResult.Ok);

            var kafka = new Mock<IProducerFactory<string, BlockedTokenModel>>();
            var producer = new Mock<IKafkaProducer<string, BlockedTokenModel>>();

            kafka.Setup(c => c.GetOrCreate(It.IsAny<string>(), null))
                .Returns(producer.Object);

            var adminRepo = FakeAdminRepositoryFactory.FakeCreateUser(AdminRepositoryResult.UserExist);
            var userService = new AdminService(adminRepo, cacheRepo, passService, kafka.Object);

            var userCreateModel = new UserCreateModel
            {
                Login = "SomeLogin",
                Password = "SomeLongPassword",
                Role = "Admin",
                UserName = "UserName"
            };

            var result = await userService.CreateUserAsync(userCreateModel, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task CreateUser_ServiceUnavailable()
        {
            var cacheRepo = new Mock<ICacheRepository>().Object;

            var passService = FakePasswordServiceFactory.FakeHashPassword(PasswordHashResult.HashResult.Ok);

            var kafka = new Mock<IProducerFactory<string, BlockedTokenModel>>();
            var producer = new Mock<IKafkaProducer<string, BlockedTokenModel>>();

            kafka.Setup(c => c.GetOrCreate(It.IsAny<string>(), null))
                .Returns(producer.Object);

            var adminRepo = FakeAdminRepositoryFactory.FakeCreateUser(AdminRepositoryResult.Error);
            var userService = new AdminService(adminRepo, cacheRepo, passService, kafka.Object);

            var userCreateModel = new UserCreateModel
            {
                Login = "SomeLogin",
                Password = "SomeLongPassword",
                Role = "Admin",
                UserName = "UserName"
            };

            var result = await userService.CreateUserAsync(userCreateModel, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.ServiceUnavailable);
        }

        [Fact]
        public async Task CreateUser_BadRequest()
        {
            var cacheRepo = new Mock<ICacheRepository>().Object;

            var passService = FakePasswordServiceFactory.FakeHashPassword(PasswordHashResult.HashResult.PasswordEmpty);

            var kafka = new Mock<IProducerFactory<string, BlockedTokenModel>>();
            var producer = new Mock<IKafkaProducer<string, BlockedTokenModel>>();

            kafka.Setup(c => c.GetOrCreate(It.IsAny<string>(), null))
                .Returns(producer.Object);

            var adminRepo = FakeAdminRepositoryFactory.FakeCreateUser(AdminRepositoryResult.Ok);
            var userService = new AdminService(adminRepo, cacheRepo, passService, kafka.Object);

            var userCreateModel = new UserCreateModel
            {
                Login = "SomeLogin",
                Password = "SomeLongPassword",
                Role = "Admin",
                UserName = "UserName"
            };

            var result = await userService.CreateUserAsync(userCreateModel, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.BadRequest);
        }
    }
}
