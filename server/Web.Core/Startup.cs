using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using Web.Core.Extensions;
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
            services.BuildLibDependencies();
            services.ConfigureCORS();
            services.ConfigureSWagger();
            services.AddMvc();
            services.AddSignalR();
            services.ConfigureDI();
            services.AddLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //if (env.IsDevelopment())
            //{
            //    loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //    loggerFactory.AddDebug();
            //}
            loggerFactory.AddLog4Net(Path.Combine(env.ContentRootPath, Configuration.GetValue<string>("Log4NetConfigFile:Name")));
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseMiddleware<DatabaseMigratorMiddleware>();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", StartupExtensions.SwaggerDocName);
                c.RoutePrefix = string.Empty;
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseCors(StartupExtensions.CorsPolicyName);
            app.UseSwagger();
            app.UseSignalR(routes =>
            {
                routes.MapHub<ReadingsHub>("/readingsHub");
            });
        }
    }
}
