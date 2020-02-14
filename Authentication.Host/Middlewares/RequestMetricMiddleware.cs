using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis.Operations;
using Prometheus;

namespace Authentication.Host.Middlewares
{
    public class RequestMetricMiddleware
    {
        private readonly RequestDelegate _request;

        private readonly List<string> ignoreRequests = new List<string> {"/metrics", "swagger"};

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
                if (!ignoreRequests.Contains(currentRequest))
                {
                    var action = httpContext.GetRouteData().Values["Action"].ToString();
                    var method = httpContext.Request.Method;

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
