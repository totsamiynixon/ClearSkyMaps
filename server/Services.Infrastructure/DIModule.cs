using AutoMapper;
using Contracts.DI;
using DAL.Intarfaces;
using Domain;
using Microsoft.Extensions.DependencyInjection;
using Services.DTO.PollutionCalculator;
using Services.DTO.Models.Reading;
using Services.Implementations;
using Services.Interfaces;
using System;

namespace Services.Infrastructure
{
    public class DIModule : IDependencyInjectorModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IApplicationConfigurationService, ApplicationConfigurationService>();
            services.AddTransient<ISensorService, SensorService>(ctx =>
            {
                return new SensorService(ctx.GetService<IDataContext>(), ctx.GetService<IPollutionLevelCalculationService>(), new Mapper(new MapperConfiguration(x =>
                {
                    x.CreateMap<SensorReadingDTO, ReadingForPollutionCalculation>();
                    x.CreateMap<Reading, ReadingForPollutionCalculation>();
                })));
            });
            services.AddTransient<IReadingService, ReadingService>(ctx =>
            {
                return new ReadingService(ctx.GetService<IDataContext>(), new Mapper(new MapperConfiguration(x =>
                {
                    x.CreateMap<SaveReadingDTO, Reading>();
                    x.CreateMap<Reading, SensorReadingDTO>();
                })));
            });
            services.AddTransient<IPollutionLevelCalculationService, PollutionLevelCalculationService>();
        }
    }
}
