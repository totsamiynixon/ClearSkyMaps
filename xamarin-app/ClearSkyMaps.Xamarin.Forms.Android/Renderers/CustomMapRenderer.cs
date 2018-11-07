using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ClearSkyMaps.Xamarin.Forms.Controls.CustomMap;
using ClearSkyMaps.Xamarin.Forms.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;
using static Android.Gms.Maps.GoogleMap;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace ClearSkyMaps.Xamarin.Forms.Droid.Renderers
{
    public class CustomMapRenderer : MapRenderer
    {
        IList<CustomMapCircle> circles;

        public CustomMapRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                // Unsubscribe
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                circles = formsMap.Circles;
                Control.GetMapAsync(this);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }

        protected override void OnMapReady(Android.Gms.Maps.GoogleMap map)
        {
            base.OnMapReady(map);
            foreach (var circle in circles)
            {
                var circleOptions = new CircleOptions();
                circleOptions.InvokeCenter(new LatLng(circle.Position.Latitude, circle.Position.Longitude));
                circleOptions.InvokeRadius(circle.Radius);
                circleOptions.InvokeFillColor(0X66FF0000);
                circleOptions.InvokeStrokeColor(0X66FF0000);
                circleOptions.Clickable(true);
                circleOptions.InvokeStrokeWidth(0);
                NativeMap.AddCircle(circleOptions);
                NativeMap.SetOnCircleClickListener(new OnCircleClickListener(circle.OnClick));
            }
        }


        public class OnCircleClickListener : Java.Lang.Object, IOnCircleClickListener
        {
            private readonly Action _onClick;
            public OnCircleClickListener(Action onClick)
            {
                _onClick = onClick;
            }

            public void OnCircleClick(Circle circle)
            {
                _onClick();
            }
        }

    }
}