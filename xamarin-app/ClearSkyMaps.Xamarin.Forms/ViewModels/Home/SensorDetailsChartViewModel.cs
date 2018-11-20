using ClearSkyMaps.Xamarin.Forms.Data;
using ClearSkyMaps.Xamarin.Forms.Delegates;
using ClearSkyMaps.Xamarin.Forms.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace ClearSkyMaps.Xamarin.Forms.ViewModels.Home
{
    public class SensorDetailsChartViewModel : ViewModelBase
    {
        public SensorDetailsChartViewModel(INavigation navigation) : base(navigation)
        {
        }
        private ObservableCollection<SensorReadingViewModel> _readings;
        public ObservableCollection<SensorReadingViewModel> Readings
        {
            get
            {
                return _readings;
            }
            set { _readings = value; }
        }
    }
}
