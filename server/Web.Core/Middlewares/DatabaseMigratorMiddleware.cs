using DAL.Intarfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Middlewares
{
    public class DatabaseMigratorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private static bool _initialized = false;

        public DatabaseMigratorMiddleware(RequestDelegate next,
                                      ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ErrorHandlerMiddleware>();
        }

        public async Task Invoke(HttpContext httpcontext, IDatabaseMigrator migrator)
        {
            if (_initialized)
            {
                await _next(httpcontext);
                return;
            }
            migrator.Migrate();
            _initialized = true;
            await _next(httpcontext);
        }
    }
}
