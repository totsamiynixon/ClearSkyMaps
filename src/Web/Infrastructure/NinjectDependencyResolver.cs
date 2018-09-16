using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using System.Web.Mvc;
using IDependencyResolverMVC = System.Web.Mvc.IDependencyResolver;
using IDependencyResolverWebAPI = System.Web.Http.Dependencies.IDependencyResolver;

namespace Web.Infrastructure
{
    public class NinjectDependencyResolver: IDependencyResolverMVC, IDependencyResolverWebAPI
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            _kernel = kernelParam;
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(this._kernel.BeginBlock());
        }

        public void Dispose()
        {
            _kernel?.Dispose();
        }
    }
}