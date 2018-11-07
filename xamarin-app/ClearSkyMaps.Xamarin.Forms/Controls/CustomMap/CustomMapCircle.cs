using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace ClearSkyMaps.Xamarin.Forms.Controls.CustomMap
{
    //public class CustomMapCircle : View
    //{
    //    private static BindableProperty AreaColorProperty =
    //       BindableProperty.Create(nameof(AreaColor), typeof(string), typeof(CustomMapCircle), "000000", propertyChanged: (a, b, c) =>
    //       {
    //       });
    //    private static BindableProperty StrokeColorProperty =
    //       BindableProperty.Create(nameof(StrokeColor), typeof(string), typeof(CustomMapCircle), "000000", propertyChanged: (a, b, c) =>
    //       {
    //       });
    //    private static BindableProperty LabelProperty =
    //       BindableProperty.Create(nameof(Label), typeof(string), typeof(CustomMapCircle), "Label", propertyChanged: (a, b, c) =>
    //       {
    //       });

    //    public Position Position { get; set; }
    //    public double Radius { get; set; }
    //    public string AreaColor
    //    {
    //        get { return (string)GetValue(AreaColorProperty); }
    //        set { SetValue(AreaColorProperty, value); }
    //    }
    //    public string StrokeColor
    //    {
    //        get { return (string)GetValue(StrokeColorProperty); }
    //        set { SetValue(StrokeColorProperty, value); }
    //    }
    //    public string Label
    //    {
    //        get { return (string)GetValue(LabelProperty); }
    //        set { SetValue(LabelProperty, value); }
    //    }

    //    public Action OnClick { get; set; }
    //}

    public class CustomMapCircle
    {


        public Position Position { get; set; }
        public double Radius { get; set; }
        public string AreaColor
        {
            get; set;
        }
        public string StrokeColor
        {
            get; set;
        }
        public string Label
        {
            get; set;
        }

        public Action OnClick { get; set; }
    }
}
