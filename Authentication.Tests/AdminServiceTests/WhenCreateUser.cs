using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Exceptions;
using Authentication.Host.Models;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Moq;
using NSV.Security.JWT;
using NSV.Security.Password;
using Xunit;

namespace Authentication.Tests.AdminServiceTests
{
    public class WhenCreateUser
    {
        [Fact]
        public async Task CreateUser_Sucess()
        {
            var passService = new Mock<IPasswordService>();

            var userRepo = FakeRepositoryFactory.CreateFakeUserRepository();
            var userService = new AdminService(userRepo, passService.Object);

            var userCreateModel = new UserCreateModel();

            var result = await userService.CreateUserAsync(userCreateModel, CancellationToken.None);

            Assert.IsType<Result<AdminResult>>(result);
            Assert.Equal(AdminResult.Ok, result.Value);
        }

        [Fact]
        public async Task CreateUser_Exist()
        {
            var passService = new Mock<IPasswordService>();

            var userRepo = FakeRepositoryFactory.CreateFakeUserRepository_Exception();

            var userService = new AdminService(userRepo, passService.Object);

            var userCreateModel = new UserCreateModel();

            var result = await userService.CreateUserAsync(userCreateModel, CancellationToken.None);


            Assert.IsType<Result<AdminResult>>(result);
            Assert.Equal(AdminResult.UserExist, result.Value);
        }
    }
}
