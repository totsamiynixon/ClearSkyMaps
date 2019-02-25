const config = {
  hubPath: "/readingsHub"
};

config.map = {};
config.map.options = {
  center: {
    lat: 53.904502,
    lng: 27.561261
  },
  zoom: 11,
  scrollwheel: false,
  mapTypeControl: false,
  styles: []
};
config.map["md-styles"] = [
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
];
config.map["ios-styles"] = [
  {
    featureType: "administrative",
    elementType: "all",
    stylers: [
      {
        saturation: "-100"
      }
    ]
  },
  {
    featureType: "administrative.province",
    elementType: "all",
    stylers: [
      {
        visibility: "off"
      }
    ]
  },
  {
    featureType: "landscape",
    elementType: "all",
    stylers: [
      {
        saturation: -100
      },
      {
        lightness: 65
      },
      {
        visibility: "on"
      }
    ]
  },
  {
    featureType: "poi",
    elementType: "all",
    stylers: [
      {
        saturation: -100
      },
      {
        lightness: "50"
      },
      {
        visibility: "simplified"
      }
    ]
  },
  {
    featureType: "road",
    elementType: "all",
    stylers: [
      {
        saturation: "-100"
      }
    ]
  },
  {
    featureType: "road.highway",
    elementType: "all",
    stylers: [
      {
        visibility: "simplified"
      }
    ]
  },
  {
    featureType: "road.arterial",
    elementType: "all",
    stylers: [
      {
        lightness: "30"
      }
    ]
  },
  {
    featureType: "road.local",
    elementType: "all",
    stylers: [
      {
        lightness: "40"
      }
    ]
  },
  {
    featureType: "transit",
    elementType: "all",
    stylers: [
      {
        saturation: -100
      },
      {
        visibility: "simplified"
      }
    ]
  },
  {
    featureType: "water",
    elementType: "geometry",
    stylers: [
      {
        hue: "#ffff00"
      },
      {
        lightness: -25
      },
      {
        saturation: -97
      }
    ]
  },
  {
    featureType: "water",
    elementType: "labels",
    stylers: [
      {
        lightness: -25
      },
      {
        saturation: -100
      }
    ]
  }
];
config.map.key = "AIzaSyAfj-ARjZc7VEGb0_grdk5VFu5wXphQyjo";

export default {
  ...config
};
