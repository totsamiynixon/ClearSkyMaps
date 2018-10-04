const config = {
  baseUrl: null,
  hubName: "readings"
};

if (process.env.NODE_ENV == "production") {
  config.baseUrl = "production_url";
} else {
  config.baseUrl = "http://localhost:56875/";
}
config.map = {};
config.map.options = {
  center: {
    lat: 53.904502,
    lng: 27.561261
  },
  zoom: 12,
  scrollwheel: false,
  mapTypeControl: false,
  styles: [
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
  ]
};
config.map.key = "AIzaSyAfj-ARjZc7VEGb0_grdk5VFu5wXphQyjo";

export default {
  ...config
};
