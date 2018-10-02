using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ErrorHandlerMiddleware(RequestDelegate next,
                                      ILoggerFactory loggerFactory,
                                      IHostingEnvironment hostingEnvironment)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ErrorHandlerMiddleware>();
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.Clear();
                context.Response.Headers.Clear();
                context.Response.StatusCode = 500;
                if (_hostingEnvironment.IsDevelopment())
                {
                    JsonResult result = new JsonResult(ex);
                    RouteData routeData = context.GetRouteData();
                    ActionDescriptor actionDescriptor = new ActionDescriptor();
                    ActionContext actionContext = new ActionContext(context, routeData ?? new RouteData(), actionDescriptor);
                    await result.ExecuteResultAsync(actionContext);
                    return;
                }
                _logger.LogError("An unhandled exception has occurred: " + ex.Message, ex);
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the error handler will not be executed.");
                }
            }
        }
    }
}
