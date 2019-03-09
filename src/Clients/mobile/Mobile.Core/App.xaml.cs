using Prism;
using Prism.Ioc;
using Mobile.Core.ViewModels;
using Mobile.Core.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mobile.Core.Interfaces;
using Mobile.Core.Implementations;
using Mobile.Core.Config;
using Unity.Lifetime;
using Unity.Injection;
using Mobile.Core.Store;
using Redux;
using Mobile.Core.Services.Interfaces;
using Mobile.Core.Services.Implementations;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Mobile.Core
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
            containerRegistry.RegisterSingleton<IApiClientService, ApiClientService>();
            containerRegistry.RegisterSingleton<AppConfig>();
            containerRegistry.RegisterInstance<IStore<AppState>>(new Store<AppState>(reducer: AppReducer.Execute, initialState: new AppState()));
            containerRegistry.RegisterSingleton<INonUITasksManager, NonUITasksManager>();
            containerRegistry.RegisterForNavigation<SensorDetailsTablePage, SensorDetailsTablePageViewModel>();
            containerRegistry.RegisterForNavigation<SensorDetailsChartPage, SensorDetailsChartPageViewModel>();
        }
    }
}
