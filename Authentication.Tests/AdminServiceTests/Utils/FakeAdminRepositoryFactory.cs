﻿using System.Collections.Generic;
using System.Threading;
using Authentication.Data.Models.Domain;
using Authentication.Data.Models.Domain.Translators;
using Authentication.Host.Repositories;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using Moq;
using NSV.Security.JWT;

namespace Authentication.Tests.AdminServiceTests.Utils
{
    public static class FakeAdminRepositoryFactory
    {
        public static IAdminRepository FakeBlockUser(AdminRepositoryResult adminRepositoryResult)
        {
            var adminRepository = new Mock<IAdminRepository>();
            var tokenList = new List<TokenModel>();

            adminRepository.Setup(c => c.BlockUserAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<AdminRepositoryResult, IEnumerable<TokenModel>>(adminRepositoryResult, tokenList));

            return adminRepository.Object;
        }

        public static IAdminRepository FakeCreateUser(AdminRepositoryResult adminRepositoryResult, User user = null)
        {
            var adminRepository = new Mock<IAdminRepository>();

            adminRepository.Setup(c => c.CreateUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<AdminRepositoryResult, UserInfo>(adminRepositoryResult, user.ToUserInfo()));

            return adminRepository.Object;
        }

        public static IAdminRepository FakeDeleteUser(AdminRepositoryResult adminRepositoryResult)
        {
            var adminRepository = new Mock<IAdminRepository>();

            adminRepository.Setup(c => c.DeleteUserAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<AdminRepositoryResult, IEnumerable<TokenModel>>(adminRepositoryResult));

            return adminRepository.Object;
        }
    }
}
