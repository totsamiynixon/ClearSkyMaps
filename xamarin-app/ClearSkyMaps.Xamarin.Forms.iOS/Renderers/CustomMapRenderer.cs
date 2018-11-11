using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ClearSkyMaps.Xamarin.Forms.Controls.CustomMap;
using ClearSkyMaps.Xamarin.Forms.iOS.Renderers;
using Foundation;
using MapKit;
using ObjCRuntime;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace ClearSkyMaps.Xamarin.Forms.iOS.Renderers
{
    public class CustomMapRenderer : MapRenderer
    {

        private CustomMap _customMap;
        private MKMapView _nativeMap;
        private MKCircleRenderer _circleRenderer;
        private Dictionary<CustomMapCircle, MKCircle> _dictionary = new Dictionary<CustomMapCircle, MKCircle>();

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                Unsubscribe((CustomMap)e.OldElement);
            }

            if (e.NewElement != null)
            {
                _customMap = (CustomMap)e.NewElement;
                _nativeMap = Control as MKMapView;
                _nativeMap.OverlayRenderer = GetOverlayRenderer;
                SetCircles(_customMap.Circles);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "Circles")
            {
                if (_nativeMap != null)
                {
                    SetCircles(_customMap.Circles);
                }
            }
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
                _nativeMap.RemoveOverlays(_dictionary.Values.ToArray());
            }
            _dictionary.Clear();
            if (circles == null)
            {
                return;
            }
            foreach (var circle in circles)
            {
                var mapCircle = MKCircle.Circle(new CoreLocation.CLLocationCoordinate2D(circle.Position.Latitude, circle.Position.Longitude), circle.Radius);
                _nativeMap.AddGestureRecognizer(new UITapGestureRecognizer((s) =>
                {
                    var clickedCircle = GetTappedOnCircleOrDefault(s, _nativeMap, _dictionary);
                    if (clickedCircle != null)
                    {
                        clickedCircle.OnClick?.Invoke();
                    }
                }));
                _dictionary.Add(circle, mapCircle);
                circle.PropertyChanged += CirclePropertyChangedHandler;
            }
        }

        private CustomMapCircle GetTappedOnCircleOrDefault(UITapGestureRecognizer tapGesture, MKMapView mapView, Dictionary<CustomMapCircle, MKCircle> dictionary)
        {
            var tappedMapView = tapGesture.View;
            var tappedPoint = tapGesture.LocationInView(tappedMapView);
            var tapped = mapView.ConvertPointFromView(tappedPoint, tappedMapView);
            MKCircle tappedOn = null;
            foreach (var mapCircle in _dictionary.Values)
            {
                var renderer = new MKCircleRenderer(mapCircle);
                renderer.InvalidatePath();
                if (renderer.Path.ContainsPoint(tapped, false))
                {
                    tappedOn = mapCircle;
                    break;
                }
            }
            return dictionary.FirstOrDefault(f => f.Value.Handle == tappedOn.Handle).Key;
        }

        private void CirclePropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            var customCircle = (CustomMapCircle)sender;
            var mapCircle = _dictionary[customCircle];
            if (e.PropertyName == "Radius")
            {
                mapCircle.SetValueForKey(new NSNumber(customCircle.Radius), new NSString("Radius"));
            }
            if (e.PropertyName == "AreaColor")
            {
                //mapCircle.Radius = customCircle.AreaColor;
            }
            if (e.PropertyName == "StrokeColor")
            {
                //mapCircle.Radius = customCircle.StrokeColor;
            }

        }

        MKOverlayRenderer GetOverlayRenderer(MKMapView mapView, IMKOverlay overlayWrapper)
        {
            if (_circleRenderer == null && !Equals(overlayWrapper, null))
            {
                var overlay = Runtime.GetNSObject(overlayWrapper.Handle) as IMKOverlay;
                _circleRenderer = new MKCircleRenderer(overlay as MKCircle)
                {
                    FillColor = UIColor.Red,
                    Alpha = 0.4f
                };
            }
            return _circleRenderer;
        }
    }
}