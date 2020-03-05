using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.LDAP;

namespace Authentication.Host.Services.ServiceExtensions
{
    public static class VaultExtensions
    { 
        public static IServiceCollection AddVault(this IServiceCollection service, Action<VaultOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentException(nameof(options));
            }

            service.AddSingleton<VaultService>();
            service.Configure(options);
            
            return service;
        }

        public static IApplicationBuilder UseVault(this IApplicationBuilder app)
        { 
            app.ApplicationServices.GetService<VaultService>().Start();

            return app;
        }
    }

    public class VaultOptions
    {
        public IAuthMethodInfo AuthMethod { get; set; }

        public string Server { get; set; } = "http://SomeAddress";

        public string Port { get; set; } = "2232";
    }

    public class VaultService
    {
        private readonly IAuthMethodInfo _info;

        private VaultClientSettings _settings;

        private readonly string _server;

        private readonly string _port;

        private readonly ILogger _logger;

        public VaultService(IOptions<VaultOptions> options, ILogger<VaultService> logger)
        {
            _server = options.Value.Server;
            _port = options.Value.Port;
            _info = options.Value.AuthMethod;
            _logger = logger;
        }

        public async void Start()
        {
            _settings = new VaultClientSettings(vaultServerUriWithPort: $"{_server}:{_port}", _info);
            var client = new VaultClient(_settings);
            _logger.LogInformation($"{_server}:{_port}");
            try
            {
                var hs = await client.V1.System.GetHealthStatusAsync();
                _logger.LogInformation(hs.ClusterName);
                _logger.LogInformation(hs.Version);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

        }
    }
}
