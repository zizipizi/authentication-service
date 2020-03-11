using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Controllers;
using Authentication.Host.Models;
using Authentication.Host.Results.Enums;
using Authentication.Host.Services;
using Authentication.Tests.AdminControllerTests.Utills;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Authentication.Tests.AdminControllerTests
{
    public class WhenCreateUser
    {
        [Fact]
        public async Task CreateUser_Success()
        {
            var adminService = FakeAdminServiceFactory.CreateUser(HttpStatusCode.OK);
            var adminController = new AdminController(adminService);
            var userModel = new UserCreateModel();

            var result = await adminController.CreateUser(userModel, CancellationToken.None);

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task CreateUser_UserExist()
        {
            var adminService = FakeAdminServiceFactory.CreateUser(HttpStatusCode.Conflict);
            var adminController = new AdminController(adminService);
            var userModel = new UserCreateModel();

            var result = await adminController.CreateUser(userModel, CancellationToken.None);

            result.Should().BeOfType<ConflictResult>();
        }

        [Fact]
        public async Task CreateUser_ServiceUnavailable()
        {
            var adminService = FakeAdminServiceFactory.CreateUser(HttpStatusCode.ServiceUnavailable);
            var adminController = new AdminController(adminService);
            var userModel = new UserCreateModel();

            var result = await adminController.CreateUser(userModel, CancellationToken.None);

            result.Should().BeOfType(typeof(StatusCodeResult));
        }
    }
}
