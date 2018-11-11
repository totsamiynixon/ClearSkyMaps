using ClearSkyMaps.Xamarin.Forms.Delegates;
using ClearSkyMaps.Xamarin.Forms.Models;
using ClearSkyMaps.Xamarin.Forms.Pages.Base;
using ClearSkyMaps.Xamarin.Forms.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microcharts;
using Entry = Microcharts.Entry;
using SkiaSharp;
using Microcharts.Forms;

namespace ClearSkyMaps.Xamarin.Forms.Pages.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChartPage : ChartPageXaml
    {
        private readonly ChartView _chartView;
        private bool IsInited;
        public ChartPage()
        {
            InitializeComponent();
            _chartView = new ChartView();

            this.Content = _chartView;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!IsInited && ViewModel != null)
            {
                _chartView.Chart = GenerateChart(ViewModel.Readings.OrderByDescending(s => s.Created).Take(10).ToArray());
                Task.Run(() =>
                {
                    ViewModel.Readings.CollectionChanged += (sender, ev) =>
                    {
                        if (ev.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                        {
                            var chart = GenerateChart(ViewModel.Readings.OrderByDescending(s => s.Created).Take(10).ToArray());
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                _chartView.Chart = chart;
                            });
                        }
                    };
                });
                IsInited = true;
            }
        }

        private LineChart GenerateChart(IEnumerable<SensorReadingViewModel> readings)
        {
            return new LineChart
            {
                Entries = readings.Select(s => new Entry(s.CO)
                {
                    Label = s.Created.ToLongTimeString(),
                    ValueLabel = s.CO.ToString(),
                    Color = SKColor.Parse("#C2C2C2")
                })
            };
        }
    }

    public abstract class ChartPageXaml : ModelBoundContentPage<SensorDetailsChartViewModel> { }
}