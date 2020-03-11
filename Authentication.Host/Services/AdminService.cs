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
using Microsoft.Extensions.Caching.Distributed;
using NSV.Security.Password;

namespace Authentication.Host.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly ICacheRepository _cacheRepository;
        private readonly IPasswordService _passwordService;

        public AdminService(IAdminRepository adminRepository, ICacheRepository cacheRepository, IPasswordService passwordService)
        {
            _adminRepository = adminRepository;
            _cacheRepository = cacheRepository;
            _passwordService = passwordService;
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

            var newUserInfo = new UserInfo
            {
                Id = createUserResult.Model.Id,
                Login = model.Login
            };

            return new Result<HttpStatusCode, UserInfo>(HttpStatusCode.OK, newUserInfo, "User created");
        }

        public async Task<Result<HttpStatusCode>> BlockUserAsync(long id, CancellationToken token)
        {
            var userBlockResult = await _adminRepository.BlockUserAsync(id, token);

            switch (userBlockResult.Value)
            {
                case AdminRepositoryResult.UserNotFound:
                    return new Result<HttpStatusCode>(HttpStatusCode.NotFound);
                case AdminRepositoryResult.Error:
                    return new Result<HttpStatusCode>(HttpStatusCode.ServiceUnavailable);
            }

            var addToBlacklistResult =  await _cacheRepository.AddRefreshTokensToBlacklistAsync(userBlockResult.Model, token);

            if (addToBlacklistResult.Value == CacheRepositoryResult.Error)
                return new Result<HttpStatusCode>(HttpStatusCode.ServiceUnavailable);

            return new Result<HttpStatusCode>(HttpStatusCode.OK);
        }

        public async Task<Result<HttpStatusCode>> DeleteUserAsync(long id, CancellationToken cancellationToken)
        {
            var deleteUserResult = await _adminRepository.DeleteUserAsync(id, cancellationToken);

            return deleteUserResult.Value == AdminRepositoryResult.UserNotFound 
                ? new Result<HttpStatusCode>(HttpStatusCode.NotFound) 
                : new Result<HttpStatusCode>(HttpStatusCode.OK);
        }
    }
}
