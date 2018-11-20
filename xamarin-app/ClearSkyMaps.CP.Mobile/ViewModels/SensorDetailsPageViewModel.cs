using ClearSkyMaps.CP.Mobile.Services.Interfaces;
using ClearSkyMaps.CP.Mobile.Store;
using ClearSkyMaps.CP.Mobile.Store.Actions;
using ClearSkyMaps.Mobile.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Redux;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClearSkyMaps.CP.Mobile.ViewModels
{
    public class SensorDetailsPageViewModel : ViewModelBase
    {
        public SensorDetailsPageViewModel(INavigationService navigationService) : base(navigationService)
        {

        }
    }
}
