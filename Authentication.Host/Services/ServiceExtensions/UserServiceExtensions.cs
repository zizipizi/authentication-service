using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Host.Services.ServiceExtensions
{
    public static class UserServiceExtensions
    {
        public static IServiceCollection AddUserService (this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUserService, UserService>();

            return serviceCollection;
        }
    }
}
