using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Android.Content;
using Android.Gms.Maps.Model;
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

        private CustomMap _customMap;
        private Dictionary<CustomMapCircle, Circle> _dictionary = new Dictionary<CustomMapCircle, Circle>();
        public CustomMapRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                Unsubscribe((CustomMap)e.OldElement);
            }

            if (e.NewElement != null)
            {
                _customMap = (CustomMap)e.NewElement;
                Control.GetMapAsync(this);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == CustomMap.CirclesProperty.PropertyName)
            {
                if (NativeMap != null)
                {
                    SetCircles(_customMap.Circles);
                }
            }
        }

        protected override void OnMapReady(Android.Gms.Maps.GoogleMap map)
        {
            base.OnMapReady(map);
            SetCircles(_customMap.Circles);
            NativeMap.SetOnCircleClickListener(new OnCircleClickListener(_dictionary));
        }

        private void Unsubscribe(CustomMap map)
        {
            map.PropertyChanged -= OnElementPropertyChanged;
            foreach (var circle in map.Circles)
            {
                circle.PropertyChanged -= CirclePropertyChangedHandler;
            }
        }

        private void SetCircles(IList<CustomMapCircle> circles)
        {
            foreach (var key in _dictionary.Keys)
            {
                _dictionary[key].Remove();
            }
            _dictionary.Clear();
            if (circles == null)
            {
                return;
            }
            foreach (var circle in circles)
            {
                var circleOptions = new CircleOptions();
                circleOptions.InvokeCenter(new LatLng(circle.Position.Latitude, circle.Position.Longitude));
                circleOptions.InvokeRadius(circle.Radius);
                circleOptions.InvokeFillColor(0X66FF0000);
                circleOptions.InvokeStrokeColor(0X66FF0000);
                circleOptions.Clickable(true);
                circleOptions.InvokeStrokeWidth(0);
                var mapCircle = NativeMap.AddCircle(circleOptions);
                _dictionary.Add(circle, mapCircle);
                circle.PropertyChanged += CirclePropertyChangedHandler;
            }
        }

        private void CirclePropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            var customCircle = (CustomMapCircle)sender;
            var mapCircle = _dictionary[customCircle];
            if (e.PropertyName == CustomMapCircle.RadiusProperty.PropertyName)
            {
                mapCircle.Radius = customCircle.Radius;
            }
            if (e.PropertyName == CustomMapCircle.AreaColorProperty.PropertyName)
            {
                //mapCircle.Radius = customCircle.AreaColor;
            }
            if (e.PropertyName == CustomMapCircle.StrokeColorProperty.PropertyName)
            {
                //mapCircle.Radius = customCircle.StrokeColor;
            }

        }

        public class OnCircleClickListener : Java.Lang.Object, IOnCircleClickListener
        {

            public readonly Dictionary<CustomMapCircle, Circle> _dictionary;
            public OnCircleClickListener(Dictionary<CustomMapCircle, Circle> dictionary)
            {
                _dictionary = dictionary;

            }
            public void OnCircleClick(Circle circle)
            {
                var mapCircle = _dictionary.FirstOrDefault(s => s.Value.Id == circle.Id).Key;
                if (mapCircle != null)
                {
                    mapCircle.OnClick();
                }
            }
        }

    }
}