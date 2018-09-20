using AutoMapper;
using AutoMapper.Configuration;
using Domain;
using Ninject;
using Ninject.Modules;
using Services.DTO.PollutionCalculator;
using Services.DTO.Reading;
using Services.Implementations;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Infrastructure
{
    public class NinjectDI : NinjectModule
    {
        public override void Load()
        {
            Bind<IApplicationConfigurationService>().To<ApplicationConfigurationService>().InTransientScope();
            Bind<IReadingService>().To<ReadingService>().InTransientScope();
            Bind<ISensorService>().To<SensorService>().InTransientScope();
            Bind<IPollutionLevelCalculationService>().To<PollutionLevelCalculationService>().InTransientScope();
            BindMappers(Kernel);
        }


        private void BindMappers(IKernel kernel)
        {
            Bind<IMapper>().ToConstant(new Mapper(new MapperConfiguration(x => {
                x.CreateMap<SensorReadingDTO, ReadingForPollutionCalculation>();
                x.CreateMap<Reading, ReadingForPollutionCalculation>();
            }))).WhenInjectedExactlyInto<SensorService>();

            Bind<IMapper>().ToConstant(new Mapper(new MapperConfiguration(x => {
                x.CreateMap<SaveReadingDTO, Reading>();
                x.CreateMap<Reading, SensorReadingDTO>();
            }))).WhenInjectedExactlyInto<ReadingService>();
        }
    }
}
