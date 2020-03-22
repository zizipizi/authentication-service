using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data.Models.Domain;
using Authentication.Host.Models;
using Authentication.Host.Repositories;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSV.Security.Password;
using Processing.ControlSystem.InternalInteractionModels.InternalAuthEvent;
using Processing.Kafka.Producer;

namespace Authentication.Host.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly ICacheRepository _cacheRepository;
        private readonly IPasswordService _passwordService;
        private readonly IProducerFactory<string, BlockedTokenModel> _kafka;
        private readonly ILogger _logger;
        public AdminService(IAdminRepository adminRepository, 
                            ICacheRepository cacheRepository, 
                            IPasswordService passwordService, 
                            IProducerFactory<string, BlockedTokenModel> kafka,
                            ILogger<AdminService> logger = null)
        {
            _adminRepository = adminRepository;
            _cacheRepository = cacheRepository;
            _passwordService = passwordService;
            _kafka = kafka;
            _logger = logger ?? new NullLogger<AdminService>();
        }

        public async Task<Result<HttpStatusCode, IEnumerable<User>>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            var result = await _adminRepository.GetAllUsersAsync(cancellationToken);

            return result.Value == AdminRepositoryResult.Ok 
                ? new Result<HttpStatusCode, IEnumerable<User>>(HttpStatusCode.OK, result.Model) 
                : new Result<HttpStatusCode, IEnumerable<User>>(HttpStatusCode.ServiceUnavailable, message: "Please, try again");
        }

        public async Task<Result<HttpStatusCode, UserInfo>> CreateUserAsync(UserCreateModel model, CancellationToken cancellationToken)
        {
            var newUserPasswordHash = _passwordService.Hash(model.Password);

            if (newUserPasswordHash.Result != PasswordHashResult.HashResult.Ok)
                return new Result<HttpStatusCode, UserInfo>(HttpStatusCode.BadRequest, message: newUserPasswordHash.Result.ToString());

            var createUserResult = await _adminRepository.CreateUserAsync(new User
            {
                Login = model.Login,
                Password = newUserPasswordHash.Hash,
                UserName = model.UserName,
                Role = model.Role.Split(",").Select(p => p.Trim())
            }, cancellationToken);

            switch (createUserResult.Value)
            {
                case AdminRepositoryResult.UserExist:
                    return new Result<HttpStatusCode, UserInfo>(HttpStatusCode.Conflict, message: "User with same login exist");
                case AdminRepositoryResult.Error:
                    return new Result<HttpStatusCode, UserInfo>(HttpStatusCode.ServiceUnavailable, message: "Please, try again");
            }

            return new Result<HttpStatusCode, UserInfo>(HttpStatusCode.OK, createUserResult.Model, "User created");
        }

        public async Task<Result<HttpStatusCode>> BlockUserAsync(long id, CancellationToken cancellationToken)
        {
            var userBlockResult = await _adminRepository.BlockUserAsync(id, cancellationToken);

            switch (userBlockResult.Value)
            {
                case AdminRepositoryResult.UserNotFound:
                    return new Result<HttpStatusCode>(HttpStatusCode.NotFound);
                case AdminRepositoryResult.Error:
                    return new Result<HttpStatusCode>(HttpStatusCode.ServiceUnavailable);
            }

            using (var message = _kafka.GetOrCreate("BlockedTokens"))
            {
                var blockedTokenModel = new BlockedTokenModel();
                foreach (var token in userBlockResult.Model)
                {
                    blockedTokenModel.AccessToken = token.AccessToken.Value;
                    blockedTokenModel.Expiration = token.AccessToken.Expiration;
                    blockedTokenModel.UserId = id;

                    var res = await message.SendAsync(token.AccessToken.Jti, blockedTokenModel);
                    if (res == ProduceResult.Failed)
                        _logger.LogError("Kafka produce failed");
                }
            }

            var addToBlacklistResult =  await _cacheRepository.AddRefreshTokensToBlacklistAsync(userBlockResult.Model, cancellationToken);

            if (addToBlacklistResult.Value == CacheRepositoryResult.Error)
                return new Result<HttpStatusCode>(HttpStatusCode.ServiceUnavailable);

            return new Result<HttpStatusCode>(HttpStatusCode.OK);
        }

        public async Task<Result<HttpStatusCode>> DeleteUserAsync(long id, CancellationToken cancellationToken)
        {
            var deleteUserResult = await _adminRepository.DeleteUserAsync(id, cancellationToken);

            switch (deleteUserResult.Value)
            {
                case AdminRepositoryResult.UserNotFound:
                    return new Result<HttpStatusCode>(HttpStatusCode.NotFound);
                case AdminRepositoryResult.Error:
                    return new Result<HttpStatusCode>(HttpStatusCode.ServiceUnavailable);
            }

            var addToBlackListResult = await _cacheRepository.AddRefreshTokensToBlacklistAsync(deleteUserResult.Model, cancellationToken);

            if (addToBlackListResult.Value == CacheRepositoryResult.Error)
                return new Result<HttpStatusCode>(HttpStatusCode.ServiceUnavailable);

            return new Result<HttpStatusCode>(HttpStatusCode.OK);
        }
    }
}
