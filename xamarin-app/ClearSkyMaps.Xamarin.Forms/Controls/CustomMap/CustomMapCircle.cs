using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace ClearSkyMaps.Xamarin.Forms.Controls.CustomMap
{
    public class CustomMapCircle : BindableObject, INotifyPropertyChanged
    {
        public static BindableProperty AreaColorProperty =
           BindableProperty.Create(nameof(AreaColor), typeof(string), typeof(CustomMapCircle), "000000", propertyChanged: (a, b, c) =>
           {
           });
        public static BindableProperty StrokeColorProperty =
           BindableProperty.Create(nameof(StrokeColor), typeof(string), typeof(CustomMapCircle), "000000", propertyChanged: (a, b, c) =>
           {
           });
        public static BindableProperty RadiusProperty =
        BindableProperty.Create(nameof(Radius), typeof(double), typeof(CustomMapCircle), ((double)50), propertyChanged: (a, b, c) =>
        {
        });

        public Position Position { get; set; }
        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        public string AreaColor
        {
            get { return (string)GetValue(AreaColorProperty); }
            set { SetValue(AreaColorProperty, value); }
        }
        public string StrokeColor
        {
            get { return (string)GetValue(StrokeColorProperty); }
            set { SetValue(StrokeColorProperty, value); }
        }

        public Action OnClick { get; set; }
    }
}
