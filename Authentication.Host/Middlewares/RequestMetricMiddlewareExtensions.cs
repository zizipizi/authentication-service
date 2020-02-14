using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
