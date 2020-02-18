using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Authentication.Host.Healthchecks
{
    public class PrometheusHealthCheckRequest
    {
        private readonly IHttpClientFactory _clientFactory;
        private string _connectionString;

        public PrometheusHealthCheckRequest(IHttpClientFactory clientFactory, Action<PrometheusHealthCheckRequest> hc)
        {
            _clientFactory = clientFactory;
        }

        public async Task<bool> IsPrometheusHealthy()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _connectionString);

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
    }
}
