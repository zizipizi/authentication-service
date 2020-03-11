using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Controllers;
using Authentication.Host.Models;
using Authentication.Host.Repositories;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Authentication.Tests.AdminControllerTests.Utills;
using Authentication.Tests.AdminServiceTests.Utils;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
            var passwordService = new Mock<IPasswordService>().Object;
            var cacheRepo = new Mock<ICacheRepository>().Object;

            var userRepo = FakeAdminRepositoryFactory.FakeDeleteUser(AdminRepositoryResult.Ok);
            var userService = new AdminService(userRepo,cacheRepo, passwordService);

            var result = await userService.DeleteUserAsync(id, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteUser_NotFound()
        {
            var id = 1;
            var passwordService = new Mock<IPasswordService>().Object;
            var cacheRepo = new Mock<ICacheRepository>().Object;

            var userRepo = FakeAdminRepositoryFactory.FakeDeleteUser(AdminRepositoryResult.UserNotFound);
            var userService = new AdminService(userRepo, cacheRepo, passwordService);

            var result = await userService.DeleteUserAsync(id, CancellationToken.None);

            result.Value.Should().BeEquivalentTo(HttpStatusCode.NotFound);
        }

    }
}
