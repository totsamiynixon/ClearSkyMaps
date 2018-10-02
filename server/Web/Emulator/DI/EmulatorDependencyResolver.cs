using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Infrastructure;

namespace Web.Emulator.DI
{
    public class EmulatorDependencyResolver : NinjectDependencyResolver
    {
        private readonly IKernel _kernel;
        public EmulatorDependencyResolver(IKernel kernel) : base(kernel)
        {
            _kernel = kernel;
        }

        public void RebuildDependencies(Action<IKernel> procedure)
        {
            procedure(_kernel);
        }
    }
}