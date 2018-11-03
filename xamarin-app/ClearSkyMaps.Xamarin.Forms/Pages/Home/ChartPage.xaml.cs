using ClearSkyMaps.Xamarin.Forms.Delegates;
using ClearSkyMaps.Xamarin.Forms.Models;
using ClearSkyMaps.Xamarin.Forms.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClearSkyMaps.Xamarin.Forms.Pages.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChartPage : ContentPage
    {
        private readonly SensorDetailsChartViewModel _vm;
        public ChartPage(Sensor sensor, SensorReadingsWasUpdatedEventHandler ev)
        {
            InitializeComponent();
            _vm = new SensorDetailsChartViewModel(sensor, ev, Navigation);
            BindingContext = _vm;
        }
    }
}