using Mobile.Core.Store;
using Mobile.Core.ViewModels;
using Prism.Common;
using Prism.Navigation;
using Redux;
using Xamarin.Forms;

namespace Mobile.Core.Views
{
    public partial class SensorDetailsPage : TabbedPage, INavigationAware
    {
        private readonly SensorDetailsTablePage TablePage;
        private readonly SensorDetailsChartPage ChartPage;
        public SensorDetailsPage(SensorDetailsTablePage tablePage, SensorDetailsChartPage chartPage)
        {
            TablePage = tablePage;
            ChartPage = chartPage;
            InitializeComponent();
            TablePage.Title = "Table";
            ChartPage.Title = "Chart";
            Children.Add(TablePage);
            Children.Add(ChartPage);

        }


        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            var tablePageVM = TablePage.BindingContext as SensorDetailsTablePageViewModel;
            var chartPageVM = ChartPage.BindingContext as SensorDetailsChartPageViewModel;
            tablePageVM.OnNavigatedTo(parameters);
            chartPageVM.OnNavigatedTo(parameters);
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {

        }
    }
}
