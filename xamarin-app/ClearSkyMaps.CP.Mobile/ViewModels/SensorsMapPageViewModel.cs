using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClearSkyMaps.CP.Mobile.ViewModels
{
    public class SensorsMapPageViewModel : ViewModelBase
    {

        public SensorsMapPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        private DelegateCommand _showDetailsCommand;
        public DelegateCommand ShowDetailsCommand =>
            _showDetailsCommand ?? (_showDetailsCommand = new DelegateCommand(ExecuteShowDetailsCommand));

        void ExecuteShowDetailsCommand()
        {
            NavigationService.NavigateAsync("SensorDetailsPage");
        }
    }
}
