using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Host.Services
{
    public static class AdminServiceExtensions
    {
        public static IServiceCollection AddAdminService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IAdminService, AdminService>();

            return serviceCollection;
        }
    }
}
