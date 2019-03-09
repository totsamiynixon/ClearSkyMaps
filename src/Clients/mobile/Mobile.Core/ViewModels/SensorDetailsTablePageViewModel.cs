using Mobile.Core.Services.Interfaces;
using Mobile.Core.Store;
using Mobile.Core.Store.Actions;
using Mobile.Models;
using Prism.Navigation;
using Redux;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Mobile.Core.ViewModels
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
