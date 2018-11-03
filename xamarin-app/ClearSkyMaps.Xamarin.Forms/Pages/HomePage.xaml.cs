using ClearSkyMaps.Xamarin.Forms.Enums;
using ClearSkyMaps.Xamarin.Forms.Pages.Home;
using ClearSkyMaps.Xamarin.Forms.Services.Interfaces;
using ClearSkyMaps.Xamarin.Forms.Store;
using ClearSkyMaps.Xamarin.Forms.Store.Home;
using ClearSkyMaps.Xamarin.Forms.Store.Home.Actions;
using ClearSkyMaps.Xamarin.Forms.ViewModels.Home;
using CommonServiceLocator;
using Redux;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClearSkyMaps.Xamarin.Forms.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private readonly IApiClientService _apiClientService;
        private readonly IStore<HomePageState> _homePageStore;
        private readonly SensorsMapViewModel _vm;
        public HomePage()
        {
            InitializeComponent();
            _apiClientService = ServiceLocator.Current.GetInstance<IApiClientService>();
            _homePageStore = ServiceLocator.Current.GetInstance<IStore<HomePageState>>();
            _vm = new SensorsMapViewModel(_homePageStore, _apiClientService, Navigation);
            BindingContext = _vm;
            Content = _vm.GetMap();
            _vm.MakeApiCall();
        }

        private void IconToolbarItem_Clicked(object sender, EventArgs e)
        {
            //_homePageStore.HomePageStore.Dispatch(new SetFilterParameterAction(Parameters.Hum));
            // Navigation.PushModalAsync(new NavigationPage(new SensorDetailsPage()));
        }
    }
}