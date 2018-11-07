using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace ClearSkyMaps.Xamarin.Forms.Controls.CustomMap
{
    public class CustomMap : Map
    {
        //private static BindableProperty PinsProperty =
        //    BindableProperty.Create(nameof(Pins), typeof(IList<Pin>), typeof(CustomMap), new List<Pin>(), propertyChanged: (a, b, c) =>
        //    {
        //        var obj = (CustomMap)a;
        //        obj.Pins = (IList<Pin>)c;
        //    });

        private static BindableProperty CirclesProperty =
          BindableProperty.Create(nameof(Pins), typeof(IList<CustomMapCircle>), typeof(CustomMap), new List<CustomMapCircle>(), propertyChanged: (a, b, c) =>
          {
              var obj = (CustomMap)a;
              obj.Circles = (IList<CustomMapCircle>)c;
          });

        //public CustomMap()
        //{
        //    SetBinding(PinsProperty, new Binding(nameof(Pins)));
        //}
        //public new IList<Pin> Pins
        //{
        //    get { return (IList<Pin>)GetValue(PinsProperty); }
        //    set { SetValue(PinsProperty, value); }
        //}

        public IList<CustomMapCircle> Circles
        {
            get { return (IList<CustomMapCircle>)GetValue(CirclesProperty); }
            set { SetValue(CirclesProperty, value); }
        }

    }
}
