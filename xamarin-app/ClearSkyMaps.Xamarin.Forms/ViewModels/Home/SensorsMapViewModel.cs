using ClearSkyMaps.Xamarin.Forms.Models;
using ClearSkyMaps.Xamarin.Forms.Pages.Home;
using ClearSkyMaps.Xamarin.Forms.Services.Interfaces;
using ClearSkyMaps.Xamarin.Forms.Store.Home;
using ClearSkyMaps.Xamarin.Forms.Store.Home.Actions;
using Redux;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace ClearSkyMaps.Xamarin.Forms.ViewModels.Home
{
    public class SensorsMapViewModel : ViewModelBase
    {
        private readonly Map _map;
        private readonly IApiClientService _apiClientService;
        private readonly IStore<HomePageState> _store;
        public SensorsMapViewModel(Map map, IStore<HomePageState> store, IApiClientService apiClientService, INavigation navigation) : base(navigation)
        {
            _map = map;
            _store = store;
            _apiClientService = apiClientService;
            InitStoreSubscriptions();
        }

        public SensorsMapViewModel(IStore<HomePageState> store, IApiClientService apiClientService, INavigation navigation) : base(navigation)
        {
            _apiClientService = apiClientService;
            _store = store;
            _map = new Map
            {
                //IsShowingUser = true,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            //_map.MoveToRegion(new MapSpan(new Position(53.902172, 27.558005), 360, 360));
            _map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(53.902172, 27.558005), Distance.FromKilometers(10)));
            InitStoreSubscriptions();
        }

        private void InitStoreSubscriptions()
        {
            _store.Subscribe(state =>
            {

                if (state.LastAction is SetSensorsAction)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        SetSensors(state.Sensors);
                    });
                }
            });
        }

        public void MakeApiCall()
        {

            Device.BeginInvokeOnMainThread(async () =>
           {
               var result = await _apiClientService.GetSensorsAsync();
               _store.Dispatch(new SetSensorsAction(result));
           });


        }
        public Map GetMap()
        {
            return _map;
        }

        //private void WaitForUIThreadExecution(Action actionToExecute)
        //{
        //    Device.BeginInvokeOnMainThread(() =>
        //    {
        //        actionToExecute();
        //    });
        //}

        public void SetSensors(IEnumerable<Sensor> sensors)
        {
            if (_map == null)
            {
                throw new Exception("Map is not setted up!");
            }
            foreach (var sensor in sensors)
            {
                _map.BatchBegin();
                var pin = new Pin
                {
                    Type = PinType.Generic,
                    Position = new Position(sensor.Latitude, sensor.Longitude),
                    Label = $"Sensor {sensor.Id}",
                    Address = "custom detail info"
                };
                pin.Clicked += (sender, args) =>
                {
                    ShowSensorDetails(sensor);
                };
                _map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(53.902172, 27.558005), Distance.FromKilometers(10)));
                _map.BatchCommit();
            }
        }

        private void ShowSensorDetails(Sensor sensor)
        {
            Navigation.PushModalAsync(new SensorDetailsPage(sensor));
        }
    }
}
