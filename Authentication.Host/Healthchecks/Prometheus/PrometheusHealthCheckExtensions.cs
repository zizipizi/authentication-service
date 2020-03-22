using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Authentication.Host.Healthchecks
{
    public static class PrometheusHealthCheckExtensions
    {
        private const string Name = "prometheues";

        public static IHealthChecksBuilder AddPrometheus(
            this IHealthChecksBuilder builder,
            string connectionString,
            string name = null,
            HealthStatus? failureStatus = null,
            IEnumerable<string> tags = null,
            TimeSpan? timout = null)
        {
            return builder.Add(new HealthCheckRegistration(name ?? Name,
                sp => new PrometheusHealthCheck(() => ServiceProviderServiceExtensions.GetRequiredService<IHttpClientFactory>(sp).CreateClient(), connectionString),
                    failureStatus,
                    tags));
        }
    }
}
