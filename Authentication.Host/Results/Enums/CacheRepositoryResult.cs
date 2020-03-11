using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Host.Results.Enums
{
    public enum CacheRepositoryResult
    {
        Ok,
        IsBlocked,
        IsNotBlocked,
        Error
    }
}
