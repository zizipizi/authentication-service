using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authentication.Host.Enums;
using Authentication.Host.Models;

namespace Authentication.Host.Services
{
    public interface IUserService
    {
        Task<Result<UserResult>> SignOut();

        Task<Result<UserResult>> ChangePassword(ChangePassModel model);
    }
}
