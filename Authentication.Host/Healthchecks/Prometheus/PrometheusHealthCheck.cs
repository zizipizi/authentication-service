using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Authentication.Host
{
    public class PrometheusHealthCheck : IHealthCheck
    {
        private readonly Func<HttpClient> _httpClientFactory;
        private readonly string _connectionString;
        public PrometheusHealthCheck(Func<HttpClient> httpClientFactory, string connectionString)
        {
            _httpClientFactory = httpClientFactory;
            _connectionString = connectionString;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _connectionString);

            var response = await _httpClientFactory().SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await Task.FromResult(HealthCheckResult.Healthy("Healthy"));
            }

            return await Task.FromResult(HealthCheckResult.Unhealthy("Unhealthy"));
        }
    }
}
