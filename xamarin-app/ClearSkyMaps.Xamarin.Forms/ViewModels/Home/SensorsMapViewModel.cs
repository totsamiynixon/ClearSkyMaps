using ClearSkyMaps.Xamarin.Forms.Controls.CustomMap;
using ClearSkyMaps.Xamarin.Forms.Models;
using ClearSkyMaps.Xamarin.Forms.Pages.Home;
using ClearSkyMaps.Xamarin.Forms.Services.Interfaces;
using ClearSkyMaps.Xamarin.Forms.Store.Home;
using ClearSkyMaps.Xamarin.Forms.Store.Home.Actions;
using Redux;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace ClearSkyMaps.Xamarin.Forms.ViewModels.Home
{
    public class SensorsMapViewModel : ViewModelBase
    {
        public SensorsMapViewModel(INavigation navigation) : base(navigation)
        {

        }

        private List<CustomMapCircle> _circles;

        public List<CustomMapCircle> Circles
        {
            get { return _circles; }
            set { SetProperty(ref _circles, nameof(Circles), value); }
        }
    }
}
