using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Readings.API.Extensions;
using Readings.API.Hubs;
using Readings.API.Middlewares;

namespace Readings.API
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
            Environment = env;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(ctx =>
            {
                if (Environment.IsDevelopment())
                {
                    Configuration["ConnectionStrings:DefaultConnection"] = $"Server={System.Environment.GetEnvironmentVariable("DATABASE_DOMAIN")};Database=ClearSkyMapsDev;User Id={System.Environment.GetEnvironmentVariable("DATABASE_USER_ID")};Password={System.Environment.GetEnvironmentVariable("DATABASE_USER_PASSWORD")}";
                    Configuration["ConnectionStrings:EmulationConnection"] = $"Server={System.Environment.GetEnvironmentVariable("DATABASE_DOMAIN")};Database=ClearSkyMapsDevEmulation;User Id={System.Environment.GetEnvironmentVariable("DATABASE_USER_ID")};Password={System.Environment.GetEnvironmentVariable("DATABASE_USER_PASSWORD")}";

                }
                return Configuration;
            });
            services.BuildLibDependencies();
            services.ConfigureCORS();
            services.ConfigureSwagger(Environment.ApplicationName);
            services.AddMvc();
            services.AddSignalR();
            services.ConfigureDI();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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
            app.UseSignalR(routes =>
            {
                routes.MapHub<ReadingsHub>("/readingsHub");
            });
        }
    }
}
