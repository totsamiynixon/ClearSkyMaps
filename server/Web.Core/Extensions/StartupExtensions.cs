using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.DependencyInjection;
using Services.DTO.Models.Reading;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Web.Core.Controllers.Api;
using Web.Core.Models.Api.Readings;

namespace Web.Core.Extensions
{
    public static class StartupExtensions
    {
        public static string CorsPolicyName => "ClearSkyMapsPolicy";
        public static string SwaggerDocName => "Clear Sky Maps Api";

        public static void BuildLibDependencies(this IServiceCollection services)
        {
            new Services.Infrastructure.DIModule().ConfigureServices(services);
            new DAL.Infrastructure.DIModule().ConfigureServices(services);
        }
        public static void ConfigureCORS(this IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy(CorsPolicyName, builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
            }));
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory(CorsPolicyName));
            });
        }
        public static void ConfigureSwagger(this IServiceCollection services, string appName)
        {
            services.AddSwaggerGen(c =>
            {
                c.DescribeAllParametersInCamelCase();
                c.DescribeStringEnumsInCamelCase();
                c.SwaggerDoc("v1", new Info
                {
                    Title = SwaggerDocName,
                    Version = "v1",
                    Description = "Clear Sky Maps project's REST API documentation."
                });
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{appName}.xml"));
            });
        }

        public static void ConfigureDI(this IServiceCollection services)
        {
            services.AddTransient<Func<Type, IMapper>>(serviceProvider => keyType =>
            {
                if (keyType == typeof(ReadingsController) || keyType == typeof(Emulator.Emulator))
                {
                    return new Mapper(new MapperConfiguration(x =>
                    {
                        x.CreateMap<ApiPostReadingModel, SaveReadingDTO>();
                        x.CreateMap<SaveReadingDTO, ApiPostReadingModel>();
                    }));
                }
                else
                {
                    throw new KeyNotFoundException("No such service to privide!");
                }
            });
            services.AddSingleton<Emulator.Emulator, Emulator.Emulator>();
        }
    }
}
