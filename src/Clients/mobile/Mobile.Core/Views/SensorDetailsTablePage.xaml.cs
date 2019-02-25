using Mobile.Core.ViewModels;
using Mobile.Models;
using Mobile.Models.Enums;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Mobile.Core.Views
{
    public partial class SensorDetailsTablePage : ContentPage
    {
        private readonly SensorDetailsTablePageViewModel VM;
        public SensorDetailsTablePage()
        {
            InitializeComponent();
            VM = BindingContext as SensorDetailsTablePageViewModel;
            Loading.IsVisible = true;
            Table.IsVisible = false;
            VM.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "Readings")
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Table.Root.Clear();
                        foreach (var item in VM.Readings)
                        {
                            Table.Root.Add(CreateTableSection(item));
                        }
                        Loading.IsVisible = false;
                        Table.IsVisible = true;
                    });
                }
            };
        }

        //protected void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        //{
        //    if (args.PropertyName == "Readings")
        //    {
        //        //Device.BeginInvokeOnMainThread(() =>
        //        //{
        //        //    Table.Root.Clear();
        //        //    foreach (var item in VM.Readings)
        //        //    {
        //        //        Table.Root.Add(CreateTableSection(item));
        //        //    }
        //        //});
        //    }
        //}

        private TableSection CreateTableSection(Reading item)
        {
            return new TableSection(item.Created.ToLongTimeString()) {
                    new TextCell
                    {
                        Text = Enum.GetName(typeof(Parameters), Parameters.CH4),
                        Detail = item.CH4.ToString(),
                    },
                    new TextCell
                    {
                        Text = Enum.GetName(typeof(Parameters), Parameters.CO),
                        Detail = item.CO.ToString()
                    },
                     new TextCell
                    {
                        Text = Enum.GetName(typeof(Parameters), Parameters.CO2),
                        Detail = item.CO2.ToString(),
                    },
                      new TextCell
                    {
                        Text = Enum.GetName(typeof(Parameters), Parameters.Dust),
                        Detail = item.Dust.ToString(),
                    }, new TextCell
                    {
                        Text = Enum.GetName(typeof(Parameters), Parameters.Hum),
                        Detail = item.Hum.ToString(),
                    },
                       new TextCell
                    {
                        Text = Enum.GetName(typeof(Parameters), Parameters.LPG),
                        Detail = item.LPG.ToString(),
                    },
                         new TextCell {
                        Text = Enum.GetName(typeof(Parameters), Parameters.Preassure),
                        Detail = item.Preassure.ToString(),
                    },
                           new TextCell {
                        Text = Enum.GetName(typeof(Parameters), Parameters.Temp),
                        Detail = item.Temp.ToString(),
                    }

                    };
        }
    }
}
