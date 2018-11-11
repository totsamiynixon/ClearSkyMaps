using ClearSkyMaps.CP.Mobile.Interfaces;
using ClearSkyMaps.CP.Mobile.Store;
using ClearSkyMaps.CP.Mobile.Store.Actions;
using ClearSkyMaps.Mobile.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Redux;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace ClearSkyMaps.CP.Mobile.ViewModels
{
    public class SensorsMapPageViewModel : ViewModelBase, INavigatedAwareAsync
    {
        private readonly Dictionary<Sensor, Circle> _sensorMapCircleDictionary;
        private readonly IApiClientService ApiService;
        private readonly IStore<AppState> Store;
        public SensorsMapPageViewModel(
            INavigationService navigationService,
            IApiClientService apiClientService,
            IStore<AppState> store) : base(navigationService)
        {
            _sensorMapCircleDictionary = new Dictionary<Sensor, Circle>();
            ApiService = apiClientService;
            Store = store;
            Store.Subscribe(state =>
            {
                if (state.LastAction is SetSensorsAction)
                {
                    var sensors = (state.LastAction as SetSensorsAction).Payload;
                    Circles.Clear();
                    foreach (var sensor in sensors)
                    {
                        var circle = new Circle
                        {
                            Center = new Position(sensor.Latitude, sensor.Longitude),
                            IsClickable = true,
                            Radius = Distance.FromMeters(50),
                            StrokeColor = Color.Red,
                            FillColor = Color.Blue,
                        };
                        circle.Clicked += (s, e) =>
                        {
                            NavigationService.NavigateAsync($"SensorDetailsPage?sensorId={sensor.Id}");
                        };
                        Circles.Add(circle);
                        _sensorMapCircleDictionary.Add(sensor, circle);
                    }

                }
            });

        }


        private ObservableCollection<Circle> _circles;

        public ObservableCollection<Circle> Circles
        {
            get { return _circles; }
            set { SetProperty(ref _circles, value); }
        }


        private DelegateCommand _showDetailsCommand;
        public DelegateCommand ShowDetailsCommand =>
            _showDetailsCommand ?? (_showDetailsCommand = new DelegateCommand(ExecuteShowDetailsCommand));

        void ExecuteShowDetailsCommand()
        {
            NavigationService.NavigateAsync("SensorDetailsPage");
        }

        public async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            var state = Store.GetState();
            if (state.Sensors == null || !state.Sensors.Any())
            {
                var result = await ApiService.GetSensorsAsync();
                Store.Dispatch(new SetSensorsAction(result));
            }
        }
    }

}
