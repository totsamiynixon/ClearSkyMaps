using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ClearSkyMaps.Xamarin.Forms.ViewModels.Home
{
    public class SensorDetailsViewModel : ViewModelBase
    {
        public SensorDetailsViewModel(INavigation navigation) : base(navigation)
        {
        }

        private bool _isTableExpanded;

        public bool IsTableExpanded
        {
            get { return _isTableExpanded; }
            set { SetProperty(ref _isTableExpanded, nameof(IsTableExpanded), value); }
        }
    }
}
