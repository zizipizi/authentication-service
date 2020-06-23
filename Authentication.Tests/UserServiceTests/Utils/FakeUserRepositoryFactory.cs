using System;
using System.Collections.Generic;
using System.Threading;
using Authentication.Data.Models.Domain;
using Authentication.Host.Repositories;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using Moq;
using NSV.Security.JWT;

namespace Authentication.Tests.UserServiceTests.Utils
{
    public class UserRepoOptions
    {
        public UserRepositoryResult GetUserByIdReturns { get; set; }

        public UserRepositoryResult UpdateUserPasswordReturns { get; set; }

        public UserRepositoryResult BlockRefreshTokenReturns { get; set; }

        public UserRepositoryResult BlockAllRefreshTokensReturns { get; set; }

        public User User { get; set; } = new User
        {
            Id = 1,
            IsActive = true,
            Login = "Login",
            Password = "Password",
        };

        public TokenModel TokenModel { get; set; } = new TokenModel(
            ("AccessToken", DateTime.Now, "accessJti"),
            ("RefreshToken", DateTime.UtcNow, "RefreshJti"));

        public IEnumerable<TokenModel> TokenModelList { get; set; } = new List<TokenModel>
        {
            new TokenModel(("AccessToken", DateTime.Now, "accessJti"),("RefreshToken", DateTime.UtcNow, "RefreshJti")),
            new TokenModel(("AccessToken", DateTime.Now, "accessJti"),("RefreshToken", DateTime.UtcNow, "RefreshJti")),
            new TokenModel(("AccessToken", DateTime.Now, "accessJti"),("RefreshToken", DateTime.UtcNow, "RefreshJti")),
            new TokenModel(("AccessToken", DateTime.Now, "accessJti"),("RefreshToken", DateTime.UtcNow, "RefreshJti"))
        };
    }

    public class UserRepoOptionsBuilder
    {
        private readonly UserRepoOptions userRepoOptions;

        public UserRepoOptionsBuilder()
        {
            userRepoOptions = new UserRepoOptions();
        }

        public UserRepoOptionsBuilder GetUserByIdReturns(UserRepositoryResult result)
        {
            userRepoOptions.GetUserByIdReturns = result;
            return this;
        }

        public UserRepoOptionsBuilder UpdateUserPasswordsReturns(UserRepositoryResult result)
        {
            userRepoOptions.UpdateUserPasswordReturns = result;
            return this;
        }

        public UserRepoOptionsBuilder BlockRefreshTokenReturns(UserRepositoryResult result)
        {
            userRepoOptions.BlockRefreshTokenReturns = result;
            return this;
        }

        public UserRepoOptionsBuilder BlockAllRefreshTokensReturns(UserRepositoryResult result)
        {
            userRepoOptions.BlockAllRefreshTokensReturns = result;
            return this;
        }

        public UserRepoOptions Build()
        {
            return userRepoOptions;
        }

    }

    public static class FakeUserRepositoryFactory
    {
        public static IUserRepository FakeUserRepo(UserRepoOptions options)
        {
            var userRepo = new Mock<IUserRepository>();

            userRepo.Setup(c => c.GetUserByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<UserRepositoryResult, User>(options.GetUserByIdReturns, options.User));

            userRepo.Setup(c =>
                    c.UpdateUserPasswordAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<UserRepositoryResult>(options.UpdateUserPasswordReturns));

            userRepo.Setup(c => c.BlockRefreshTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<UserRepositoryResult, TokenModel>(options.BlockRefreshTokenReturns,
                    options.TokenModel));

            userRepo.Setup(c => c.BlockAllRefreshTokensAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new Result<UserRepositoryResult, IEnumerable<TokenModel>>(options.BlockAllRefreshTokensReturns,
                        options.TokenModelList));

            return userRepo.Object;
        }
    }
}
