using Prism.Navigation;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace ClearSkyMaps.CP.Mobile.Views
{
    public partial class SensorsMapPage : ContentPage, INavigationAware
    {
        public SensorsMapPage()
        {
            InitializeComponent();
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            //throw new System.NotImplementedException();
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            //if (!(Parent is NavigationPage) && Parent != null)
            //{
            //    return;
            //}
            //var navPage = Parent as NavigationPage;
            //var prevColor = navPage.BarBackgroundColor;
            //Map.CameraMoveStarted += (s, ev) =>
            //{
            //    navPage.BarBackgroundColor = Color.Transparent;
            //};
            //Map.CameraIdled += (s, ev) =>
            //{
            //    navPage.BarBackgroundColor = prevColor;
            //};
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
            //throw new System.NotImplementedException();
        }
    }
}
