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
    public partial class TablePage : ContentPage
    {
        private readonly SensorDetailsTableViewModel _vm;
        public TablePage(Sensor sensor, SensorReadingsWasUpdatedEventHandler ev)
        {
            InitializeComponent();
            _vm = new SensorDetailsTableViewModel(sensor, ev, Navigation);
            BindingContext = _vm;
        }

    }
}