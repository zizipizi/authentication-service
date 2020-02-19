using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Prometheus;

namespace Authentication.Host.Middlewares
{
    public class RequestMetricMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly List<string> ignoreRequests = new List<string> { "metrics", "swagger" };

        public RequestMetricMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var currentRequest = httpContext.Request.Path.Value;

            try
            {
                await _next.Invoke(httpContext);
            }
            finally
            {
                if (!ignoreRequests.Contains(currentRequest))
                {
                    var action = httpContext.GetRouteData().Values["Action"].ToString();
                    var method = httpContext.Request.Method;

                    var counter = Metrics.CreateCounter($"auth_{action}_total", "Metrics from auth service",
                        new CounterConfiguration
                        {
                            LabelNames = new[] { "action", "method" }
                        });
                    counter.Labels(action, method).Inc();
                }
            }
        }
    }
}