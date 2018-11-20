using ClearSkyMaps.CP.Mobile.Config;
using ClearSkyMaps.CP.Mobile.Interfaces;
using ClearSkyMaps.CP.Mobile.Services.Interfaces;
using ClearSkyMaps.CP.Mobile.Store;
using ClearSkyMaps.CP.Mobile.Store.Actions;
using ClearSkyMaps.Mobile.Models;
using ClearSkyMaps.Mobile.Models.Enums;
using ClearSkyMaps.Mobile.Models.Hub;
using Microsoft.AspNetCore.SignalR.Client;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Redux;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace ClearSkyMaps.CP.Mobile.ViewModels
{
    public class SensorsMapPageViewModel : ViewModelBase, INavigationAware, IDestructible
    {
        private readonly Dictionary<Sensor, Circle> SensorMapCircleDictionary;
        private readonly IApiClientService ApiService;
        private readonly IStore<AppState> Store;
        private readonly HubConnection HubConnection;
        private readonly INonUITasksManager NonUITasksManager;
        public SensorsMapPageViewModel(
            INavigationService navigationService,
            IApiClientService apiClientService,
            IStore<AppState> store,
            AppConfig config,
            INonUITasksManager nonUITasksManager) : base(navigationService)
        {
            ApiService = apiClientService;
            Store = store;
            SensorMapCircleDictionary = new Dictionary<Sensor, Circle>();
            HubConnection = new HubConnectionBuilder().WithUrl($"{config.BaseServiceUrl}/readingsHub").Build();
            NonUITasksManager = nonUITasksManager;
            NonUITasksManager.BeginInvokeInNonUIThread(() =>
            {
                HubConnection.On<HubDispatchModel>("DispatchReadingAsync", (reading) =>
                {
                    Store.Dispatch(new UpdateSensorAction(reading.Reading, reading.SensorId, reading.LatestPollutionLevel));
                });
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
                            SensorMapCircleDictionary.Add(sensor, circle);
                        }
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            foreach (var circle in SensorMapCircleDictionary)
                            {
                                Circles.Add(circle.Value);
                            }
                        });
                    }
                    if (state.LastAction is UpdateSensorAction)
                    {
                        var action = (state.LastAction as UpdateSensorAction);
                        var circle = SensorMapCircleDictionary.FirstOrDefault(f => f.Key.Id == action.SensorId).Value;
                        if (circle == null)
                        {
                            return;
                        }
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            switch (action.PollutionLevel)
                            {
                                case PollutionLevels.Low:
                                    circle.FillColor = Color.Green;
                                    circle.StrokeColor = Color.GreenYellow;
                                    break;
                                case PollutionLevels.Medium:
                                    circle.FillColor = Color.Yellow;
                                    circle.StrokeColor = Color.YellowGreen;
                                    break;
                                case PollutionLevels.High:
                                    circle.FillColor = Color.Red;
                                    circle.StrokeColor = Color.DarkRed;
                                    break;
                            }
                        });
                    }
                });
            });
        }


        private ObservableCollection<Circle> _circles;

        public ObservableCollection<Circle> Circles
        {
            get { return _circles ?? (_circles = new ObservableCollection<Circle>()); }
            set { SetProperty(ref _circles, value); }
        }


        private DelegateCommand _showDetailsCommand;
        public DelegateCommand ShowDetailsCommand =>
            _showDetailsCommand ?? (_showDetailsCommand = new DelegateCommand(ExecuteShowDetailsCommand));

        void ExecuteShowDetailsCommand()
        {
            NavigationService.NavigateAsync("SensorDetailsPage");
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            NonUITasksManager.BeginInvokeInNonUIThread(async () =>
           {
               var state = Store.GetState();
               await HubConnection.StartAsync();
               if (state.Sensors == null || !state.Sensors.Any())
               {
                   var result = await ApiService.GetSensorsAsync();
                   Store.Dispatch(new SetSensorsAction(result));
               }
           });
        }

        public override void Destroy()
        {
            base.Destroy();
            NonUITasksManager.BeginInvokeInNonUIThread(async () =>
            {
                await HubConnection.StopAsync();
            });
        }
    }

}
