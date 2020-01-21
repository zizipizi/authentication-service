using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Host.Enums
{
    public enum AuthResult
    {
        Ok,
        WrongLoginOrPass,
        UserBlocked,
        UserNotFound,
        TokenValidationProblem
    }
}
