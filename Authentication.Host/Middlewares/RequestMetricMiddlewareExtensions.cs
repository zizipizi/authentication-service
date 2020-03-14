using Microsoft.AspNetCore.Builder;

namespace Authentication.Host.Middlewares
{
    public static class RequestMetricMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestCounterMetrics(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestMetricMiddleware>();
        }
    }
}
