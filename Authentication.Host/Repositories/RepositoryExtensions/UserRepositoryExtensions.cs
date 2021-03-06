﻿using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Host.Repositories.RepositoryExtensions
{
    public static class UserRepositoryExtensions
    {
        public static IServiceCollection AddUserRepository(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUserRepository, UserRepository>();

            return serviceCollection;
        }
    }
}
