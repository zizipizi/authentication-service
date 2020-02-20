using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Host.Repositories.RepositoryExtensions
{
    public static class TokenRepositoryExtensions
    {
        public static IServiceCollection AddTokenRepository(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ITokenRepository, TokenRepository>();

            return serviceCollection;
        }
    }
}
