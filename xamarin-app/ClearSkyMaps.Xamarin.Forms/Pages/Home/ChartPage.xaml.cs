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
        public ChartPage()
        {
            InitializeComponent();
            ToolbarItems.Clear();
            var abc = this;
            this.ToolbarItems.Clear();
            if (this.Parent != null)
            {
                var parent = (NavigationPage)this.Parent;
                parent.ToolbarItems.Clear();
            }
        }
    }
}