using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Moq;
using NSV.Security.JWT;
using NSV.Security.Password;
using Xunit;

namespace Authentication.Tests.AdminServiceTests
{
    public class WhenDeleteUser
    {
        [Fact]
        public async Task DeleteUser_Success()
        {
            var id = 1;
            var jwtService = new Mock<IJwtService>();
            var passwordService = new Mock<IPasswordService>();

            var userRepo = FakeRepositoryFactory.DeleteFakeUserRepository();
            var userService = new AdminService(userRepo, passwordService.Object, jwtService.Object);

            var result = await userService.DeleteUserAsync(id, CancellationToken.None);

            Assert.IsType<Result<AdminResult>>(result);
            Assert.Equal(AdminResult.Ok, result.Value);
        }

        [Fact]
        public async Task DeleteUser_NotFound()
        {
            var id = 1;
            var jwtService = new Mock<IJwtService>();
            var passwordService = new Mock<IPasswordService>();

            var userRepo = FakeRepositoryFactory.DeleteFakeUserRepository_Exception();
            var userService = new AdminService(userRepo, passwordService.Object, jwtService.Object);

            var result = await userService.DeleteUserAsync(id, CancellationToken.None);

            Assert.IsType<Result<AdminResult>>(result);
            Assert.Equal(AdminResult.UserNotFound, result.Value);
        }

    }
}
