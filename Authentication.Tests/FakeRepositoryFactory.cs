using Authentication.Data.Exceptions;
using Authentication.Data.Models.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Host.Repositories;
using Authentication.Host.Services;
using NSV.Security.JWT;

namespace Authentication.Tests
{
    public static class FakeRepositoryFactory
    {
        public static IUserRepository CreateFakeUser()
        {
            var userRepositoryFake = new Mock<IUserRepository>();
            userRepositoryFake.Setup(c => c.CreateUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(It.IsAny<long>()));

            return userRepositoryFake.Object;
        }

        public static IUserRepository CreateFakeUser_Exception()
        {
            var userRepositoryFake = new Mock<IUserRepository>();
            userRepositoryFake.Setup(c => c.CreateUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Throws(new EntityNotFoundException("User with same login exist"));

            return userRepositoryFake.Object;
        }

        public static IUserRepository BlockFakeUser()
        {
            var userRepositoryFake = new Mock<IUserRepository>();
            userRepositoryFake.Setup(c => c.BlockUserAsync(It.IsAny<long>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            return userRepositoryFake.Object;
        }

        public static IUserRepository BlockFakeUser_Exception()
        {
            var userRepositoryFake = new Mock<IUserRepository>();
            userRepositoryFake.Setup(c => c.BlockUserAsync(It.IsAny<long>(), CancellationToken.None))
                .Throws(new EntityNotFoundException("User not found"));

            return userRepositoryFake.Object;
        }

        public static IUserRepository DeleteFakeUser()
        {
            var userRepositoryFake = new Mock<IUserRepository>();
            userRepositoryFake.Setup(c => c.DeleteUserAsync(It.IsAny<long>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            return userRepositoryFake.Object;
        }

        public static IUserRepository DeleteFakeUser_Exception()
        {
            var userRepositoryFake = new Mock<IUserRepository>();
            userRepositoryFake.Setup(c => c.DeleteUserAsync(It.IsAny<long>(), CancellationToken.None))
                .Throws(new EntityNotFoundException("User not found"));

            return userRepositoryFake.Object;
        }

        public static IUserRepository SignIn()
        {
            var userRepositoryFake = new Mock<IUserRepository>();
            userRepositoryFake.Setup(c => c.GetUserByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new User{UserName = "UserName", Login = "UserName", IsActive = true, Id = 1}));

            return userRepositoryFake.Object;
        }

        public static IUserRepository SignIn_EntityException()
        {
            var userRepositoryFake = new Mock<IUserRepository>();
            userRepositoryFake.Setup(c => c.GetUserByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Throws(new EntityNotFoundException("User not found"));

            return userRepositoryFake.Object;
        }

        public static IUserRepository SignIn_Exception()
        {
            var userRepositoryFake = new Mock<IUserRepository>();
            userRepositoryFake.Setup(c => c.GetUserByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception("Some exception"));

            return userRepositoryFake.Object;
        }

        public static IUserRepository RefreshToken_Ok()
        {
            var userRepositoryFake = new Mock<IUserRepository>();
            userRepositoryFake.Setup(c => c.CheckRefreshTokenAsync(It.IsAny<JwtTokenResult>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            return userRepositoryFake.Object;
        }

        public static IUserRepository RefreshToken_EntityException()
        {
            var userRepositoryFake = new Mock<IUserRepository>();
            userRepositoryFake.Setup(c =>
                    c.CheckRefreshTokenAsync(It.IsAny<JwtTokenResult>(), It.IsAny<CancellationToken>()))
                .Throws(new EntityNotFoundException("Token is blocked"));

            return userRepositoryFake.Object;
        }

        public static IUserRepository RefreshToken_Exception()
        {
            var userRepositoryFake = new Mock<IUserRepository>();
            userRepositoryFake.Setup(c => c.CheckRefreshTokenAsync(It.IsAny<JwtTokenResult>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception("DB Error"));

            return userRepositoryFake.Object;
        }

        public static IUserRepository SignOut_Ok()
        {
            var userRepositoryFake = new Mock<IUserRepository>();
            userRepositoryFake.Setup(c => c.BlockAllTokensAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            return userRepositoryFake.Object;
        }

        public static IUserRepository SignOut_EntityException()
        {
            var userRepositoryFake = new Mock<IUserRepository>();
            userRepositoryFake.Setup(c => c.BlockAllTokensAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .Throws(new EntityNotFoundException("Error"));

            return userRepositoryFake.Object;
        }

        public static IUserRepository SignOut_Exception()
        {
            var userRepositoryFake = new Mock<IUserRepository>();
            userRepositoryFake.Setup(c => c.BlockAllTokensAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception("Error"));

            return userRepositoryFake.Object;
        }

        public static IUserRepository ChangePassword_Ok()
        {
            var userRepositoryFake = new Mock<IUserRepository>();
            userRepositoryFake.Setup(c => c.GetUserByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new User {UserName = "UserName", Password = "Password", Login = "Login"}));

            return userRepositoryFake.Object;
        }

        public static IUserRepository ChangePassword_EntityException()
        {
            var userRepositoryFake = new Mock<IUserRepository>();
            userRepositoryFake.Setup(c => c.GetUserByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .Throws(new EntityNotFoundException("Error"));

            return userRepositoryFake.Object;
        }

        public static IUserRepository ChangePassword_Exception()
        {
            var userRepositoryFake = new Mock<IUserRepository>();
            userRepositoryFake.Setup(c => c.GetUserByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception("Error"));

            return userRepositoryFake.Object;
        }
    }
}
