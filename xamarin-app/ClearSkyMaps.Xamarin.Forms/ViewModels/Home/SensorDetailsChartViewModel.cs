using ClearSkyMaps.Xamarin.Forms.Delegates;
using ClearSkyMaps.Xamarin.Forms.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ClearSkyMaps.Xamarin.Forms.ViewModels.Home
{
    public class SensorDetailsChartViewModel : ViewModelBase
    {
        private readonly Sensor _sensor;
        public SensorDetailsChartViewModel(Sensor sensor, SensorReadingsWasUpdatedEventHandler ev, INavigation navigation) : base(navigation)
        {
            _sensor = sensor;
            ev += (reading) =>
            {
                Readings.Add(reading);
            };
        }

        public ICollection<Reading> Readings { get; private set; }
    }
}
