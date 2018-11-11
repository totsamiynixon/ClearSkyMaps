using AutoMapper;
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClearSkyMaps.Xamarin.Forms.Pages.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SensorDetailsPage : TabbedPage
    {
        private static IMapper _mapper = new Mapper(new MapperConfiguration(x => x.CreateMap<Reading, SensorReadingViewModel>()));

        private IStore<HomePageState> _store;
        private Sensor _sensor;
        private readonly TablePage _tablePage;
        private readonly ChartPage _chartPage;
        private readonly SensorDetailsTableViewModel _tableVM;
        private readonly SensorDetailsChartViewModel _chartVM;
        private readonly ObservableCollection<SensorReadingViewModel> _collection;
        private readonly Thread _collectionUpdateThread;
        public SensorDetailsPage()
        {
            InitializeComponent();
            _collection = new ObservableCollection<SensorReadingViewModel>();
            _chartPage = new ChartPage();
            _tablePage = new TablePage();
            if (_tableVM == null)
            {
                _tableVM = new SensorDetailsTableViewModel(Navigation);
                _tableVM.Readings = _collection;
            }
            if (_chartVM == null)
            {
                _chartVM = new SensorDetailsChartViewModel(Navigation);
                _chartVM.Readings = _collection;
            }
            _tablePage.BindingContext = _tableVM;
            _chartPage.BindingContext = _chartVM;
            Children.Add(_tablePage);
            Children.Add(_chartPage);
            Task.Run(() =>
            {
                _store = ServiceLocator.Current.GetService<IStore<HomePageState>>();
                _store.Subscribe(state =>
                {
                    if (_sensor != null && state.LastAction is UpdateSensorAction)
                    {
                        var action = (state.LastAction as UpdateSensorAction);
                        if (_sensor.Id == action.SensorId)
                        {
                            if (_tableVM != null || _chartVM != null)
                            {
                                var item = _mapper.Map<Reading, SensorReadingViewModel>(action.Payload);
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    _collection.Add(item);
                                });
                            }
                        }
                    }
                });
            });
        }


        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _sensor = null;
        }

        public void SetSensor(Sensor sensor)
        {
            _sensor = null;
            Device.BeginInvokeOnMainThread(() =>
            {
                _collection.Clear();
            });
            var readings = _mapper.Map<List<Reading>, List<SensorReadingViewModel>>(sensor.Readings);
            foreach (var item in readings)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    _collection.Add(item);
                });
            }
            _sensor = sensor;
            _tableVM.Title = $"Sensor {_sensor.Id}!";
        }
    }
}