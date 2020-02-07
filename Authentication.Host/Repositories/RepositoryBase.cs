using Authentication.Data.Models;
using Microsoft.Extensions.Logging;

namespace Authentication.Host.Repositories
{
    public abstract class RepositoryBase
    {
        protected readonly AuthContext _context;
        protected readonly ILogger _logger;

        protected RepositoryBase(AuthContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}