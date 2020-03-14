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
