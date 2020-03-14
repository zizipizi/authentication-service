using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Host.Services.ServiceExtensions
{
    public static class AuthServiceExtensions
    {
        public static IServiceCollection AddAuthService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IAuthService, AuthService>();

            return serviceCollection;
        }
    }
}
