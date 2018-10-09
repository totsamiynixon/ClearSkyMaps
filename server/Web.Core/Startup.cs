﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Web.Core.Hubs;
using WebUI.Middlewares;

namespace Web.Core
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
           .SetBasePath(env.ContentRootPath)
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
           .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(ctx =>
            {
                return Configuration;
            });
            BuildLibDependencies(services);
            ConfigureCORS(services);
            services.AddMvc();
            services.AddSignalR();
            services.AddSingleton<Emulator.Emulator, Emulator.Emulator>();
        }

        private void BuildLibDependencies(IServiceCollection services)
        {
            new Services.Infrastructure.DIModule().ConfigureServices(services);
            new DAL.Infrastructure.DIModule().ConfigureServices(services);
        }

        private void ConfigureCORS(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("ClearSkyMapsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyHeader()
                       .AllowCredentials();
            }));
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("ClearSkyMapsPolicy"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseMiddleware<DatabaseMigratorMiddleware>();

            //if (env.IsDevelopment())
            //{
            //    app.UseBrowserLink();
            //    //app.UseDeveloperExceptionPage();
            //    //app.UseDatabaseErrorPage();
            //}
            if (!env.IsDevelopment())
            {
                var options = new RewriteOptions().Add(context =>
                {
                    var request = context.HttpContext.Request;
                    var response = context.HttpContext.Response;
                    var pathValue = request.Path.Value;
                    if (!pathValue.StartsWith("/api"))
                    {
                        request.Path = new PathString("/api" + request.Path.Value);
                    }
                });
                app.UseRewriter(options);
            }
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            if ((env.IsDevelopment()))
            {
                app.UseCors("ClearSkyMapsPolicy");
            }
            app.UseSignalR(routes =>
            {
                routes.MapHub<ReadingsHub>("/readings");
            });
        }
    }
}
