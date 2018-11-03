using ClearSkyMaps.Xamarin.Forms.Delegates;
using ClearSkyMaps.Xamarin.Forms.Models;
using ClearSkyMaps.Xamarin.Forms.Store;
using ClearSkyMaps.Xamarin.Forms.Store.Home;
using ClearSkyMaps.Xamarin.Forms.Store.Home.Actions;
using ClearSkyMaps.Xamarin.Forms.ViewModels.Home;
using CommonServiceLocator;
using Redux;
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
        public event SensorReadingsWasUpdatedEventHandler OnSensorReadingsUpdated;
        private readonly IStore<HomePageState> _store;
        public SensorDetailsPage(Sensor sensor)
        {
            _store = ServiceLocator.Current.GetService<IStore<HomePageState>>();
            InitializeComponent();
            Children.Add(new TablePage(sensor, OnSensorReadingsUpdated));
            Children.Add(new ChartPage(sensor, OnSensorReadingsUpdated));
            _store.Subscribe(state =>
            {
                if (state.LastAction is UpdateSensorAction)
                {
                    var action = (state.LastAction as UpdateSensorAction);
                    if (sensor.Id == action.SensorId)
                    {
                        OnSensorReadingsUpdated(action.Payload);
                    }
                }
            });

        }
    }
}