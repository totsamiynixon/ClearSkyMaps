using AutoMapper;
using ClearSkyMaps.Xamarin.Forms.Data;
using ClearSkyMaps.Xamarin.Forms.Delegates;
using ClearSkyMaps.Xamarin.Forms.Models;
using ClearSkyMaps.Xamarin.Forms.Pages.Home;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace ClearSkyMaps.Xamarin.Forms.ViewModels.Home
{
    public class SensorDetailsTableViewModel : ViewModelBase
    {
        public SensorDetailsTableViewModel(INavigation navigation) : base(navigation)
        {
        }

        private bool _isTableExpanded;

        public bool IsTableExpanded
        {
            get { return _isTableExpanded; }
            set { SetProperty(ref _isTableExpanded, nameof(IsTableExpanded), value); }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, nameof(Title), value); }
        }

        private ObservableCollection<SensorReadingViewModel> _readings;

        public ObservableCollection<SensorReadingViewModel> Readings
        {
            get
            {
                return _readings;
            }
            set { SetProperty(ref _readings, nameof(Readings), value); }
        }


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
        public float CO2 { get; set; }

        public float LPG { get; set; }

        public float CO { get; set; }

        public float CH4 { get; set; }

        public float Dust { get; set; }

        public float Temp { get; set; }

        public float Hum { get; set; }

        public float Preassure { get; set; }

        public DateTime Created { get; set; }
    }

}


