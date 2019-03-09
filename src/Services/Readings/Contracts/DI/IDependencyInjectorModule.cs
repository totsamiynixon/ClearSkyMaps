using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Readings.Contracts.DI
{
    public interface IDependencyInjectorModule
    {
        void ConfigureServices(IServiceCollection services);
    }
}
