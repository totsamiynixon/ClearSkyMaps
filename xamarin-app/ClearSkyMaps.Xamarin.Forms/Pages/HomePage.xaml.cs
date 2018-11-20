using ClearSkyMaps.Xamarin.Forms.Config;
using ClearSkyMaps.Xamarin.Forms.Controls;
using ClearSkyMaps.Xamarin.Forms.Controls.CustomMap;
using ClearSkyMaps.Xamarin.Forms.Enums;
using ClearSkyMaps.Xamarin.Forms.Models;
using ClearSkyMaps.Xamarin.Forms.Models.Hub;
using ClearSkyMaps.Xamarin.Forms.Pages.Base;
using ClearSkyMaps.Xamarin.Forms.Pages.Home;
using ClearSkyMaps.Xamarin.Forms.Services.Interfaces;
using ClearSkyMaps.Xamarin.Forms.Store;
using ClearSkyMaps.Xamarin.Forms.Store.Home;
using ClearSkyMaps.Xamarin.Forms.Store.Home.Actions;
using ClearSkyMaps.Xamarin.Forms.ViewModels.Home;
using CommonServiceLocator;
using Microsoft.AspNetCore.SignalR.Client;
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
    public partial class HomePage : HomePageXaml
    {
        private readonly IApiClientService _apiClientService;
        private readonly IStore<HomePageState> _homePageStore;
        private bool IsInited { get; set; }
        private HubConnection connection;
        public HomePage(IApiClientService apiClientService, IStore<HomePageState> store, AppConfig config)
        {
            InitializeComponent();
            Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(53.903192, 27.558389), Distance.FromKilometers(5)));
            _apiClientService = apiClientService;
            _homePageStore = store;
            Task.Factory.StartNew(async () =>
            {
                _homePageStore.Subscribe(state =>
                {

                    if (ViewModel != null && state.LastAction is SetSensorsAction)
                    {
                        var action = state.LastAction as SetSensorsAction;
                        var pageCache = new SensorDetailsPage();
                        ViewModel.Circles = action.Payload.Select(sensor => new CustomMapCircle
                        {
                            Position = new Position(sensor.Latitude, sensor.Longitude),
                            AreaColor = "ffffff",
                            StrokeColor = "000000",
                            Radius = 50,
                            OnClick = async () =>
                            {
                                if (!IsBusy)
                                {
                                    IsBusy = true;
                                    pageCache.SetSensor(sensor);
                                    await Navigation.PushAsync(pageCache);
                                }
                            }
                        }).ToList();
                    }
                });
                connection = new HubConnectionBuilder()
                 .WithUrl($"{config.BaseServiceUrl}/readingsHub")
                 .Build();
                connection.On<HubDispatchModel>("DispatchReadingAsync", (reading) =>
                {
                    _homePageStore.Dispatch(new UpdateSensorAction(reading.Reading, reading.SensorId, reading.LatestPollutionLevel));
                });
                var result = await _apiClientService.GetSensorsAsync();
                _homePageStore.Dispatch(new SetSensorsAction(result));
            });
            this.BindingContext = new SensorsMapViewModel(Navigation);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!IsInited)
            {
                await connection.StartAsync();
            }
            IsBusy = false;
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            //await DisplayAlert("Abc", "Abc", "Close");
            //await Navigation.PushAsync(new TestPage(new Page()), false);
        }
    }


    public abstract class HomePageXaml : ModelBoundContentPage<SensorsMapViewModel> { }
}