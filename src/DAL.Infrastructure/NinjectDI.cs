using DAL.Implementations.Contexts;
using DAL.Intarfaces;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Infrastructure
{
    public class NinjectDI : NinjectModule
    {
        public override void Load()
        {
            Bind<IDataContext>().To<DataContext>();
        }
    }
}
