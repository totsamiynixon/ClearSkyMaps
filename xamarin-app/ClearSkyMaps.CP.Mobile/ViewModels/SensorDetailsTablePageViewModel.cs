using ClearSkyMaps.CP.Mobile.Services.Interfaces;
using ClearSkyMaps.CP.Mobile.Store;
using ClearSkyMaps.CP.Mobile.Store.Actions;
using ClearSkyMaps.Mobile.Models;
using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Redux;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace ClearSkyMaps.CP.Mobile.ViewModels
{
    public class SensorDetailsTablePageViewModel : ViewModelBase
    {
        private readonly IStore<AppState> Store;
        private readonly INonUITasksManager NonUITasksManager;
        private int? SensorId { get; set; }
        public SensorDetailsTablePageViewModel(INavigationService navigationService,
            IStore<AppState> store,
            INonUITasksManager nonUITasksManager) : base(navigationService)
        {
            Store = store;
            NonUITasksManager = nonUITasksManager;
            NonUITasksManager.BeginInvokeInNonUIThread(() =>
            {
                Store.Subscribe(state =>
                {
                    if (SensorId != null && state.LastAction is UpdateSensorAction)
                    {
                        var action = (state.LastAction as UpdateSensorAction);
                        if (action.SensorId == SensorId.Value)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Readings = state.GetSensorById(SensorId.Value).Readings;
                            });
                        }
                    }
                });
            });
        }

        private List<Reading> _readings;

        public List<Reading> Readings
        {
            get { return _readings; }
            set { SetProperty(ref _readings, value); }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters["sensorId"] == null)
            {
                throw new Exception("Sensor Id must be set up!");
            }
            var sensorId = Int32.TryParse((string)parameters["sensorId"], out var tempVal) ? tempVal : (int?)null;
            if (!sensorId.HasValue)
            {
                throw new Exception("Sensor Id must be set up!");
            }
            SensorId = sensorId.Value;
            Device.BeginInvokeOnMainThread(() =>
            {
                Readings = Store.GetState().GetSensorById(sensorId.Value).Readings;
            });
            base.OnNavigatedTo(parameters);
        }
    }
}
