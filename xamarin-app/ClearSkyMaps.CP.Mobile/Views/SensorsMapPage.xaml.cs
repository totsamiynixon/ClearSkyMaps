using ClearSkyMaps.CP.Mobile.ViewModels;
using Prism.Navigation;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace ClearSkyMaps.CP.Mobile.Views
{
    public partial class SensorsMapPage : ContentPage, INavigationAware, IDestructible
    {
        private ViewModelBase ViewModel;
        public SensorsMapPage()
        {
            InitializeComponent();
            Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(53.903192, 27.558389), Distance.FromKilometers(5)));
        }

        public void Destroy()
        {
            
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            //throw new System.NotImplementedException();
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
            ViewModel = BindingContext as ViewModelBase;
            if (ViewModel == null)
            {
                return;
            }
            if (ViewModel is SensorsMapPageViewModel)
            {
                var vm = (SensorsMapPageViewModel)ViewModel;
                foreach (var circle in vm.Circles)
                {
                    Map.Circles.Add(circle);
                }
                vm.Circles.CollectionChanged += (s, e) =>
                {
                    if (e.Action == NotifyCollectionChangedAction.Add)
                    {
                        Map.Circles.Add(e.NewItems[0] as Circle);
                    };
                    if (e.Action == NotifyCollectionChangedAction.Remove)
                    {
                        foreach (Circle item in e.OldItems)
                        {
                            Map.Circles.Remove(item);
                        }
                    };
                    if (e.Action == NotifyCollectionChangedAction.Reset)
                    {
                        Map.Circles.Clear();
                    }
                };
            }
        }
    }
}
