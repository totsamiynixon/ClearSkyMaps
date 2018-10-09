const config = {
  hubName: "readings"
};
if (process.env.NODE_ENV == "production") {
  config.baseUrl = "http://api.clearskymaps.totsamiynixon.com";
} else if (process.env.NODE_ENV == "staging") {
  config.baseUrl = "http://api.staging.clearskymaps.totsamiynixon.com";
} else {
  config.baseUrl = "http://localhost:56875/api/";
}

config.map = {};
config.map.options = {
  center: {
    lat: 53.904502,
    lng: 27.561261
  },
  zoom: 11,
  scrollwheel: false,
  mapTypeControl: false,
  styles: [
    {
      featureType: "landscape.natural",
      stylers: [
        {
          color: "#bcddff"
        }
      ]
    },
    {
      featureType: "road.highway",
      elementType: "geometry.fill",
      stylers: [
        {
          color: "#5fb3ff"
        }
      ]
    },
    {
      featureType: "road.arterial",
      stylers: [
        {
          color: "#ebf4ff"
        }
      ]
    },
    {
      featureType: "road.local",
      elementType: "geometry.fill",
      stylers: [
        {
          color: "#ebf4ff"
        }
      ]
    },
    {
      featureType: "road.local",
      elementType: "geometry.stroke",
      stylers: [
        {
          visibility: "on"
        },
        {
          color: "#93c8ff"
        }
      ]
    },
    {
      featureType: "landscape.man_made",
      elementType: "geometry",
      stylers: [
        {
          color: "#c7e2ff"
        }
      ]
    },
    {
      featureType: "transit.station.airport",
      elementType: "geometry",
      stylers: [
        {
          saturation: 100
        },
        {
          gamma: 0.82
        },
        {
          hue: "#0088ff"
        }
      ]
    },
    {
      elementType: "labels.text.fill",
      stylers: [
        {
          color: "#1673cb"
        }
      ]
    },
    {
      featureType: "road.highway",
      elementType: "labels.icon",
      stylers: [
        {
          saturation: 58
        },
        {
          hue: "#006eff"
        }
      ]
    },
    {
      featureType: "poi",
      elementType: "geometry",
      stylers: [
        {
          color: "#4797e0"
        }
      ]
    },
    {
      featureType: "poi.park",
      elementType: "geometry",
      stylers: [
        {
          color: "#209ee1"
        },
        {
          lightness: 49
        }
      ]
    },
    {
      featureType: "transit.line",
      elementType: "geometry.fill",
      stylers: [
        {
          color: "#83befc"
        }
      ]
    },
    {
      featureType: "road.highway",
      elementType: "geometry.stroke",
      stylers: [
        {
          color: "#3ea3ff"
        }
      ]
    },
    {
      featureType: "administrative",
      elementType: "geometry.stroke",
      stylers: [
        {
          saturation: 86
        },
        {
          hue: "#0077ff"
        },
        {
          weight: 0.8
        }
      ]
    },
    {
      elementType: "labels.icon",
      stylers: [
        {
          hue: "#0066ff"
        },
        {
          weight: 1.9
        }
      ]
    },
    {
      featureType: "poi",
      elementType: "geometry.fill",
      stylers: [
        {
          hue: "#0077ff"
        },
        {
          saturation: -7
        },
        {
          lightness: 24
        }
      ]
    }
  ]
};
config.map.key = "AIzaSyAfj-ARjZc7VEGb0_grdk5VFu5wXphQyjo";

export default {
  ...config
};
