using ClearSkyMaps.Xamarin.Forms.Controls;
using ClearSkyMaps.Xamarin.Forms.Controls.CustomMap;
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
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace ClearSkyMaps.Xamarin.Forms.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private readonly IApiClientService _apiClientService;
        private readonly IStore<HomePageState> _homePageStore;
        private SensorsMapViewModel _vm { get; set; }
        private CustomMap _map { get; set; }
        public HomePage(IApiClientService apiClientService, IStore<HomePageState> store)
        {
            InitializeComponent();
            _apiClientService = apiClientService;
            _homePageStore = store;
        }

        private void IconToolbarItem_Clicked(object sender, EventArgs e)
        {
            //_homePageStore.HomePageStore.Dispatch(new SetFilterParameterAction(Parameters.Hum));
            // Navigation.PushModalAsync(new NavigationPage(new SensorDetailsPage()));
        }

        protected override async void OnAppearing()
        {
            List<CustomMapCircle> _circles = null;
            //await Task.Factory.StartNew(async () =>
            //{
            var result = await _apiClientService.GetSensorsAsync();
            _homePageStore.Dispatch(new SetSensorsAction(result));
            _circles = result.Select(sensor => new CustomMapCircle
            {
                Position = new Position(sensor.Latitude, sensor.Longitude),
                Label = $"Sensor with id {sensor.Id}",
                AreaColor = "ffffff",
                StrokeColor = "000000",
                Radius = 50,
                OnClick = async () => { await DisplayAlert(sensor.Id.ToString(), sensor.LatestPollutionLevel.ToString(), "Close"); }
            }).ToList();
            _map = new CustomMap
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Circles = _circles
            };
            this.Content = _map;
            //});
            _vm = new SensorsMapViewModel(Navigation);
            this.BindingContext = _vm;
            _map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(53.903192, 27.558389), Distance.FromKilometers(5)));
            _homePageStore.Subscribe(state =>
            {

                if (state.LastAction is SetSensorsAction)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        //var sensors = (state.LastAction as SetSensorsAction).Payload;
                        //_vm.Circles = sensors.Select(sensor => new CustomMapCircle
                        //{
                        //    Position = new Position(sensor.Latitude, sensor.Longitude),
                        //    Label = $"Sensor with id {sensor.Id}",
                        //    AreaColor = "ffffff",
                        //    StrokeColor = "000000",
                        //    Radius = 5000,
                        //    OnClick = async () => { await DisplayAlert(sensor.Id.ToString(), sensor.LatestPollutionLevel.ToString(), "Close"); }
                        //}).ToList();
                    });
                }
            });
        }
    }
}