using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Data;
using Authentication.Data.Models.Domain;
using Authentication.Data.Models.Entities;
using Authentication.Host.Models;
using Authentication.Host.Results;
using Authentication.Host.Results.Enums;
using NSV.Security.JWT;

namespace Authentication.Host.Services
{
    public interface IAdminService
    {
        Task<IEnumerable<User>> GetAll();

        Task<Result<AdminResult>> CreateUserAsync(UserCreateModel model, CancellationToken token);

        Task<Result<AdminResult>> BlockUserAsync(int id, CancellationToken token);

        Task<Result<AdminResult>> DeleteUserAsync(int id, CancellationToken token);
    }
}