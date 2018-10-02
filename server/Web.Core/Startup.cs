using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddMvc();
            services.AddSignalR();
            services.AddSingleton<Emulator.Emulator, Emulator.Emulator>();
        }

        private void BuildLibDependencies(IServiceCollection services)
        {
            new Services.Infrastructure.DIModule().ConfigureServices(services);
            new DAL.Infrastructure.DIModule().ConfigureServices(services);
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

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseSignalR(routes =>
            {
                routes.MapHub<ReadingsHub>("/readings");
            });
        }
    }
}
