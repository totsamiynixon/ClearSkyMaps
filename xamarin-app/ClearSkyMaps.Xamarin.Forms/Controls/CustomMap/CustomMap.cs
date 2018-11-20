using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace ClearSkyMaps.Xamarin.Forms.Controls.CustomMap
{
    public class CustomMap : Map, INotifyPropertyChanged
    {

        public static BindableProperty CirclesProperty =
          BindableProperty.Create(nameof(Circles), typeof(IList<CustomMapCircle>), typeof(CustomMap), new List<CustomMapCircle>(), propertyChanged: (a, b, c) =>
          {
              var obj = (CustomMap)a;
              obj.Circles = (IList<CustomMapCircle>)c;
          });

        public IList<CustomMapCircle> Circles
        {
            get { return (IList<CustomMapCircle>)GetValue(CirclesProperty); }
            set { SetValue(CirclesProperty, value); }
        }

    }
}
