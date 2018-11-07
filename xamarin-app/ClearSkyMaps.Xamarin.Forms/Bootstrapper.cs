using ClearSkyMaps.Xamarin.Forms.Config;
using ClearSkyMaps.Xamarin.Forms.Pages;
using ClearSkyMaps.Xamarin.Forms.Services.Implementations;
using ClearSkyMaps.Xamarin.Forms.Services.Interfaces;
using ClearSkyMaps.Xamarin.Forms.Store.Home;
using CommonServiceLocator;
using Redux;
using System;
using System.Collections.Generic;
using System.Text;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.ServiceLocation;
using Xamarin.Forms;

namespace ClearSkyMaps.Xamarin.Forms
{
    public static class Bootstrapper
    {
        private static void RegisterPlatformDependencies(UnityContainer container)
        {
            //container.RegisterInstance<INavigation>(DependencyService.Get<INavigation>());
        }

        private static void RegisterAppDependencies(UnityContainer container)
        {
            container.RegisterType<AppConfig>(
                new ContainerControlledLifetimeManager(),
                new InjectionFactory(c => AppConfigInitializer.InitializeConfig()));
            container.RegisterType<IApiClientService, ApiClientService>();
            container.RegisterType<HomePage>();
            container.RegisterSingleton<IStore<HomePageState>>(new InjectionFactory(c => new Store<HomePageState>(reducer: HomePageReducer.Execute, initialState: new HomePageState())));
        }

        public static void RegisterDependencies()
        {
            var unityContainer = new UnityContainer();
            var unityServiceLocator = new UnityServiceLocator(unityContainer);
            RegisterAppDependencies(unityContainer);
            RegisterPlatformDependencies(unityContainer);
            ServiceLocator.SetLocatorProvider(() => unityServiceLocator);
        }
    }
}
