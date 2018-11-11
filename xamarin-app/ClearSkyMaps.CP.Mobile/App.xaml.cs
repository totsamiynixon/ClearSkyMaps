using Prism;
using Prism.Ioc;
using ClearSkyMaps.CP.Mobile.ViewModels;
using ClearSkyMaps.CP.Mobile.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ClearSkyMaps.CP.Mobile.Interfaces;
using ClearSkyMaps.CP.Mobile.Implementations;
using ClearSkyMaps.CP.Mobile.Config;
using Unity.Lifetime;
using Unity.Injection;
using ClearSkyMaps.CP.Mobile.Store;
using Redux;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ClearSkyMaps.CP.Mobile
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("MainPage/NavigationPage/SensorsMapPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Pages
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<SensorsMapPage, SensorsMapPageViewModel>();
            containerRegistry.RegisterForNavigation<SensorDetailsPage, SensorDetailsPageViewModel>();

            //Services
            containerRegistry.Register<IApiClientService, ApiClientService>();
            containerRegistry.RegisterSingleton<AppConfig>();
            containerRegistry.RegisterInstance<IStore<AppState>>(new Store<AppState>(reducer: AppReducer.Execute, initialState: new AppState()));
        }
    }
}
