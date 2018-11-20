using AutoMapper;
using ClearSkyMaps.Xamarin.Forms.Delegates;
using ClearSkyMaps.Xamarin.Forms.Enums;
using ClearSkyMaps.Xamarin.Forms.Models;
using ClearSkyMaps.Xamarin.Forms.Pages.Base;
using ClearSkyMaps.Xamarin.Forms.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClearSkyMaps.Xamarin.Forms.Pages.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TablePage : TablePageXaml
    {
        private bool IsInited { get; set; }
        public TablePage()
        {
            InitializeComponent();
            Table.Intent = TableIntent.Form;
            Table.RowHeight = -1;
            Table.HeightRequest = -2;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModel != null)
            {
                if (!IsInited)
                {
                    Task.Run(() =>
                    {
                        ViewModel.Readings.CollectionChanged += (sender, e) =>
                        {
                            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null && Table.Root != null)
                            {
                                foreach (var newItem in e.NewItems)
                                {
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        Table.Root.Clear();
                                        foreach (var item in ViewModel.Readings.OrderByDescending(s => s.Created).Take(10))
                                        {
                                            Table.Root.Add(CreateTableSection(item));
                                        }
                                    });
                                }
                            }
                            if (e.Action == NotifyCollectionChangedAction.Reset)
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    Table.Root.Clear();
                                });
                            }
                        };
                    });
                    Table.Root = new TableRoot("Table");
                    foreach (var item in ViewModel.Readings.OrderByDescending(s => s.Created))
                    {
                        Table.Root.Add(CreateTableSection(item));
                    }
                    IsInited = true;
                }
            }
        }

        private TableSection CreateTableSection(SensorReadingViewModel item)
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


    public abstract class TablePageXaml : ModelBoundContentPage<SensorDetailsTableViewModel> { }
}