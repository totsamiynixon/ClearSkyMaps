using AutoMapper;
using ClearSkyMaps.Xamarin.Forms.Delegates;
using ClearSkyMaps.Xamarin.Forms.Models;
using ClearSkyMaps.Xamarin.Forms.Pages.Home;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ClearSkyMaps.Xamarin.Forms.ViewModels.Home
{
    public class SensorDetailsTableViewModel : ViewModelBase
    {
        private readonly Sensor _sensor;
        private static IMapper _mapper;
        public SensorDetailsTableViewModel(Sensor sensor, SensorReadingsWasUpdatedEventHandler ev, INavigation navigation) : base(navigation)
        {
            _sensor = sensor;
            SetReadings(_sensor.Readings);
            ev += (reading) =>
            {
                Readings.Add(_mapper.Map<Reading, SensorReadingViewModel>(reading));
            };
        }

        private void SetReadings(IEnumerable<Reading> readings)
        {
            if (_mapper == null)
            {
                _mapper = new Mapper(new MapperConfiguration(x => x.CreateMap<Reading, SensorReadingViewModel>()));
            }
            Readings = _mapper.Map<IEnumerable<Reading>, ICollection<SensorReadingViewModel>>(readings);
        }

        private bool _isTableExpanded;

        public bool IsTableExpanded
        {
            get { return _isTableExpanded; }
            set { SetProperty(ref _isTableExpanded, nameof(IsTableExpanded), value); }
        }

        public ICollection<SensorReadingViewModel> Readings { get; private set; }


        private Command _expandCollapseTable;

        public Command ExpandCollapseTable
        {
            get
            {
                return _expandCollapseTable ?? (_expandCollapseTable = new Command(() => IsTableExpanded = !IsTableExpanded));
            }
        }
    }

    public class SensorReadingViewModel
    {
        public float CO2 { get; }

        public float LPG { get; }

        public float CO { get; }

        public float CH4 { get; }

        public float Dust { get; }

        public float Temp { get; }

        public float Hum { get; }

        public float Preassure { get; }

        public DateTime Created { get; }
    }

}


