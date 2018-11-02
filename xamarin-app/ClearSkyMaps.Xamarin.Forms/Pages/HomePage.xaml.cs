using ClearSkyMaps.Xamarin.Forms.Pages.Home;
using ClearSkyMaps.Xamarin.Forms.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClearSkyMaps.Xamarin.Forms.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            StoreManager.InitHomePageStore();
        }

        private void IconToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new NavigationPage(new SensorDetailsPage()));
        }
    }
}