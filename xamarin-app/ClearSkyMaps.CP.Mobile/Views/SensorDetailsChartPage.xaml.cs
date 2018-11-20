using ClearSkyMaps.CP.Mobile.ViewModels;
using ClearSkyMaps.Mobile.Models;
using Microcharts;
using Microcharts.Forms;
using SkiaSharp;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace ClearSkyMaps.CP.Mobile.Views
{
    public partial class SensorDetailsChartPage : ContentPage
    {
        private readonly SensorDetailsChartPageViewModel VM;
        private readonly ChartView ChartView;
        public SensorDetailsChartPage()
        {
            InitializeComponent();
            VM = BindingContext as SensorDetailsChartPageViewModel;
            ChartView = new ChartView();

            this.Content = ChartView;
            VM.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Readings")
                {
                    var chart = GenerateChart(VM.Readings.OrderByDescending(s => s.Created).Take(10).ToArray());
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ChartView.Chart = chart;
                    });
                }
            };
        }

        private LineChart GenerateChart(IEnumerable<Reading> readings)
        {
            return new LineChart
            {
                Entries = readings.Select(s => new Microcharts.Entry(s.CO)
                {
                    Label = s.Created.ToLongTimeString(),
                    ValueLabel = s.CO.ToString(),
                    Color = SKColor.Parse("#C2C2C2")
                })
            };
        }
    }
}
