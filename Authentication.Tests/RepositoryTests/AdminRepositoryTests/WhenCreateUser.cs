using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Models.Domain;
using Authentication.Host.Repositories;
using Authentication.Host.Results.Enums;
using FluentAssertions;
using Xunit;

namespace Authentication.Tests.RepositoryTests.AdminRepositoryTests
{
    public class WhenCreateUser
    {
        [Fact]
        public async Task CreateUser_Ok()
        {
            var user = new User
            {
                Login = "Login",
                Password = "Password",
                IsActive = true,
                UserName = "UserName",
                Role = new List<string> { "Admin", "User" }
            };

            var authContext = FakeContextFactory.CreateUser_Ok();

            var adminRepository = new AdminRepository(authContext);

            var createUserResult = await adminRepository.CreateUserAsync(user, CancellationToken.None);

            var dbResult = authContext.Users.FirstOrDefault(c => c.Login == "Login");

            createUserResult.Value.Should().BeEquivalentTo(AdminRepositoryResult.Ok);
            createUserResult.Model.Login.Should().BeEquivalentTo("Login");

            dbResult.Login.Should().BeEquivalentTo("Login");
            dbResult.Password.Should().BeEquivalentTo("Password");
        }

        [Fact]
        public async Task CreateUser_UserExistWithSameLogin()
        {
            var user = new User
            {
                Login = "Login",
                Password = "Password",
                IsActive = true,
                UserName = "UserName",
                Role = new List<string> { "Admin", "User" }
            };

            var secondUser = new User
            {
                Login = "Login",
                Password = "Password",
                IsActive = true,
                UserName = "UserName",
                Role = new List<string> { "Admin", "User" }
            };

            var authContext = FakeContextFactory.CreateUser_UserExist();

            var adminRepository = new AdminRepository(authContext);

            await adminRepository.CreateUserAsync(user, CancellationToken.None);

            var secondUserCreate = await adminRepository.CreateUserAsync(secondUser, CancellationToken.None);

            secondUserCreate.Value.Should().BeEquivalentTo(AdminRepositoryResult.UserExist);
        }
    }
}
