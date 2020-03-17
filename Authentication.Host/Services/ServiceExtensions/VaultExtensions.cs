using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VaultSharp;
using VaultSharp.V1.AuthMethods;

namespace Authentication.Host.Services.ServiceExtensions
{
    // В дальнейшем разнести всё
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

            var g = app.ApplicationServices.GetService<IHttpClientFactory>();

            var client = g.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "http://10.200.38.165:8200/v1/backend/test/anno");
            request.Headers.Add("X-Vault-Token", "e9XCjS9RmEHGOtx9/XvyFgpe7xvPclvcbcUve50xMO72");

            var response = client.SendAsync(request).Result;
            var body = response.Content.ReadAsStringAsync().Result;
            var js = JsonConvert.DeserializeObject<object>(body);

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

            //var y = client.Settings.AuthMethodInfo.ReturnedLoginAuthInfo.ClientToken;
            //var g = client.Settings.AuthMethodInfo.ReturnedLoginAuthInfo;
            //var s = client.V1.Auth.Token.LookupSelfAsync();
            _logger.LogInformation($"{_server}:{_port}");
            //var p = await client.V1.Secrets.KeyValue.V1.ReadSecretAsync("backend/test/anno");
            //var secrets = await client.V1.Secrets.KeyValue.V1.ReadSecretAsync("backend/test/anno/ENVIRONMENT", mountPoint: "kv");
            //var a = await client.V1.Secrets.KeyValue.V1.ReadSecretPathsAsync("test/anno", mountPoint: "kv");
            //var b = await client.V1.Secrets.KeyValue.V1.ReadSecretPathsAsync("backend/test/anno", mountPoint: "kv");
            //var c = await client.V1.Secrets.KeyValue.V1.ReadSecretPathsAsync("backend/test/anno");
            //var d = await client.V1.Secrets.KeyValue.V1.ReadSecretPathsAsync("test/anno");

            //var e = await client.V1.Secrets.KeyValue.V2.ReadSecretPathsAsync("test/anno", mountPoint: "kv");
            //var f = await client.V1.Secrets.KeyValue.V2.ReadSecretPathsAsync("backend/test/anno", mountPoint: "kv");
            //var g = await client.V1.Secrets.KeyValue.V2.ReadSecretPathsAsync("backend/test/anno");
            //var h = await client.V1.Secrets.KeyValue.V2.ReadSecretPathsAsync("test/anno");

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
