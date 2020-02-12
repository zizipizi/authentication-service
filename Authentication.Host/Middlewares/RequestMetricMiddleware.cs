using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Prometheus;

namespace Authentication.Host.Middlewares
{
    public class RequestMetricMiddleware
    {
        private readonly RequestDelegate _request;

        public RequestMetricMiddleware(RequestDelegate request)
        {
            _request = request;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var currentRequest = httpContext.Request.Path.Value;
            try
            {
                await _request.Invoke(httpContext);
            }
            finally
            {
                if (currentRequest != "/metrics" && currentRequest != "/swagger")
                {
                    var action = httpContext.GetRouteData().Values["Action"].ToString();
                    var method = httpContext.Request.Method;
                    var p = httpContext.Request.Headers;
                    var counter = Metrics.CreateCounter($"auth_{action}_total", "Metrics from auth service", new CounterConfiguration
                    {
                        LabelNames = new[] { "action", "method" }
                    });
                    counter.Labels(action, method).Inc();

                }
            }
        }
    }
}
