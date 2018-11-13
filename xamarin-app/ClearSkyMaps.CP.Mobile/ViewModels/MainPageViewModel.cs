using ClearSkyMaps.CP.Mobile.Config;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ClearSkyMaps.CP.Mobile.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly AppConfig _appConfig;
        public MainPageViewModel(INavigationService navigationService, AppConfig appConfig) : base(navigationService)
        {
            _appConfig = appConfig;
        }

        private DelegateCommand<object> _emulationCommand;
        public DelegateCommand<object> EmulationCommand =>
            _emulationCommand ?? (_emulationCommand = new DelegateCommand<object>(ExecuteEmulationCommand));

        async void ExecuteEmulationCommand(object param)
        {
            var command = (string)param;
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri($"{_appConfig.BaseServiceUrl}/api/emulation/");
                await httpClient.GetAsync("command");
            }
        }
    }
}
