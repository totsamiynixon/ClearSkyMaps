using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Resolvers;

namespace Web.Infrastructure
{
    public class NinjecSignalRDependencyResolver : DefaultDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjecSignalRDependencyResolver(IKernel kernel)
        {
            if (kernel == null)
            {
                throw new ArgumentNullException(nameof(kernel));
            }

            _kernel = kernel;
            AddBindings(kernel);
        }

        public override object GetService(Type serviceType)
        {
            var service = _kernel.TryGet(serviceType) ?? base.GetService(serviceType);
            return service;
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            var services = _kernel.GetAll(serviceType).Concat(base.GetServices(serviceType));
            return services;
        }

        private void AddBindings(IKernel kernel)
        {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new SignalRContractResolver();
            var serializer = JsonSerializer.Create(settings);
            kernel.Bind<JsonSerializer>().ToConstant(serializer);
        }
    }
}