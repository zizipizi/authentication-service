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

namespace Authentication.Tests
{
    public static class FakeRepositoryFactory
    {
        public static IUserRepository CreateFakeUserRepository()
        {
            var userRepositoryFake = new Mock<IUserRepository>();
            userRepositoryFake.Setup(c => c.CreateUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            return userRepositoryFake.Object;
        }

        public static IUserRepository CreateFakeUserRepository_Exception()
        {
            var userRepositoryFake = new Mock<IUserRepository>();
            userRepositoryFake.Setup(c => c.CreateUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Throws(new EntityNotFoundException("User not found"));

            return userRepositoryFake.Object;
        }

        public static IUserRepository BlockFakeUserRepository()
        {
            var userRepositoryFake = new Mock<IUserRepository>();
            userRepositoryFake.Setup(c => c.BlockUserAsync(It.IsAny<long>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            return userRepositoryFake.Object;
        }

        public static IUserRepository BlockFakeUserRepository_Exception()
        {
            var userRepositoryFake = new Mock<IUserRepository>();
            userRepositoryFake.Setup(c => c.BlockUserAsync(It.IsAny<long>(), CancellationToken.None))
                .Throws(new EntityNotFoundException("User not found"));

            return userRepositoryFake.Object;
        }

        public static IUserRepository DeleteFakeUserRepository()
        {
            var userRepositoryFake = new Mock<IUserRepository>();
            userRepositoryFake.Setup(c => c.DeleteUserAsync(It.IsAny<long>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            return userRepositoryFake.Object;
        }

        public static IUserRepository DeleteFakeUserRepository_Exception()
        {
            var userRepositoryFake = new Mock<IUserRepository>();
            userRepositoryFake.Setup(c => c.DeleteUserAsync(It.IsAny<long>(), CancellationToken.None))
                .Throws(new EntityNotFoundException("User not found"));

            return userRepositoryFake.Object;
        }
    }
}
