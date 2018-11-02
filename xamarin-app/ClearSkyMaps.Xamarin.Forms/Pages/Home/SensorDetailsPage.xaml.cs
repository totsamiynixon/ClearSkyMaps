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
    public partial class SensorDetailsPage : TabbedPage
    {
        private readonly SensorDetailsViewModel _vm;
        public SensorDetailsPage()
        {
            InitializeComponent();
            _vm = new SensorDetailsViewModel(Navigation);
            BindingContext = _vm;
            //if (this.CurrentPage != null && this.CurrentPage is NavigationPage)
            //{
            //    this.CurrentPage.ToolbarItems.Clear();
            //    NavigationPage.SetHasNavigationBar(this.CurrentPage, false);
            //}
            //NavigationPage.SetHasNavigationBar(this, false);
            //this.ToolbarItems.Clear();
            var abc = this;

        }

        public new SensorDetailsViewModel BindingContext
        {
            set
            {
                base.BindingContext = value;
                base.OnPropertyChanged("BindingContext");
            }
        }

        private void IconExpand_Clicked(object sender, EventArgs e)
        {
            var iconBtn = (ToolbarItem)sender;
            _vm.IsTableExpanded = !_vm.IsTableExpanded;
            iconBtn.Text = _vm.IsTableExpanded ? "fas-compress" : "fas-expand";
        }
    }
}