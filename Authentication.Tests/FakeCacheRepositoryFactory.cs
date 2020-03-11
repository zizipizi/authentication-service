using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Authentication.Host.Repositories;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using Moq;
using NSV.Security.JWT;

namespace Authentication.Tests
{
    public static class FakeCacheRepositoryFactory
    {
        public static ICacheRepository FakeAddRefreshTokensToBlacklistAsync(CacheRepositoryResult result)
        {
            var cacheRepository = new Mock<ICacheRepository>();

            cacheRepository.Setup(c =>
                    c.AddRefreshTokensToBlacklistAsync(It.IsAny<IEnumerable<TokenModel>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<CacheRepositoryResult>(result));

            return cacheRepository.Object;
        }

        public static ICacheRepository FakeIsRefreshTokenBlockedAsync(CacheRepositoryResult result)
        {
            var cacheRepository = new Mock<ICacheRepository>();

            cacheRepository.Setup(c =>
                    c.IsRefreshTokenBlockedAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<CacheRepositoryResult>(result));

            return cacheRepository.Object;
        }

        public static ICacheRepository FakeCacheRepository(CacheRepositoryResult isRefreshTokenBlockedResult, CacheRepositoryResult AddRefreshTokenResult)
        {
            var cacheRepository = new Mock<ICacheRepository>();

            cacheRepository.Setup(c =>
                    c.IsRefreshTokenBlockedAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<CacheRepositoryResult>(isRefreshTokenBlockedResult));

            cacheRepository.Setup(c =>
                    c.AddRefreshTokensToBlacklistAsync(It.IsAny<IEnumerable<TokenModel>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<CacheRepositoryResult>(AddRefreshTokenResult));

            return cacheRepository.Object;
        }
    }
}
