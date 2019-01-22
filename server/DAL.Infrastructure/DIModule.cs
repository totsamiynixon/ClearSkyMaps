using Contracts.DI;
using DAL.Implementations;
using DAL.Implementations.Contexts;
using DAL.Intarfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
namespace DAL.Infrastructure
{
    public class DIModule : IDependencyInjectorModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IDataContext, DataContext>(ctx => BuildContext(ctx));
            services.AddTransient<DataContext, DataContext>(ctx => BuildContext(ctx));
            services.AddTransient<IDatabaseMigrator, DatabaseMigrator>(ctx =>
            {
                return new DatabaseMigrator(ctx.GetService<DataContext>());
            });
        }

        private DataContext BuildContext(IServiceProvider ctx)
        {
            var emulationEnabled = ctx.GetService<IConfiguration>().GetValue<bool?>("emulationEnabled") ?? false;
            var builder = new DbContextOptionsBuilder<DataContext>();
            if (emulationEnabled)
            {

                builder.UseSqlServer(ctx.GetService<IConfiguration>().GetConnectionString("EmulationConnection"));
            }
            else
            {
                builder.UseSqlServer(ctx.GetService<IConfiguration>().GetConnectionString("DefaultConnection"));
            }
            return new DataContext(builder.Options);
        }
    }
}