using Ninject.Modules;
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
        }
    }
}
