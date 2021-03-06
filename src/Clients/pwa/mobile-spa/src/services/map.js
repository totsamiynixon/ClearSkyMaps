import config from "@/config";
import GoogleMapsLoader from "google-maps";
import rich_marker_js from "googlemaps-js-rich-marker";

var google,
  map,
  areas,
  current,
  autocompleteService,
  placeService,
  directionsService,
  directionsDisplay,
  geocoder;
function initMap(el, theme, onLoad) {
  GoogleMapsLoader.release();
  google, (map = null);
  areas = {};
  GoogleMapsLoader.KEY = config.map.key;
  GoogleMapsLoader.LIBRARIES = ["geometry", "places"];
  GoogleMapsLoader.LANGUAGE = "ru";
  GoogleMapsLoader.load(function(google) {
    config.map.options.styles =
      config.map[(theme.ios ? "ios" : "md") + "-styles"];
    map = new google.maps.Map(el, config.map.options);
    autocompleteService = new google.maps.places.AutocompleteService();
    placeService = new google.maps.places.PlacesService(map);
    directionsService = new google.maps.DirectionsService();
    directionsDisplay = new google.maps.DirectionsRenderer();
    geocoder = new google.maps.Geocoder();
  });
  GoogleMapsLoader.onLoad(googleLib => {
    google = googleLib;
    onLoad();
  });
}

function createNewArea(pollutionLevel, lat, long, markerText, onMarkerClick) {
  let guid = createGuid();
  areas[guid] = {};
  let position = new google.maps.LatLng(lat, long);
  areas[guid].circle = new google.maps.Circle({
    strokeColor: getStrokeColorByPollutionLevel(pollutionLevel),
    strokeOpacity: 0.8,
    strokeWeight: 2,
    fillColor: getFillColorByPollutionLevel(pollutionLevel),
    fillOpacity: 0.35,
    map: map,
    center: position,
    radius: 1000
  });
  areas[guid].marker = new rich_marker_js.RichMarker({
    position: position,
    map: map,
    content: generateMarker(markerText),
    shadow: "none"
  });
  areas[guid].marker.setOptions({ opacity: 0.8 });
  areas[guid].marker.addListener("click", onMarkerClick);
  return guid;
}

function updateArea(id, pollutionLevel, markerText) {
  areas[id].circle.setOptions({
    strokeColor: getStrokeColorByPollutionLevel(pollutionLevel),
    fillColor: getFillColorByPollutionLevel(pollutionLevel)
  });
  areas[id].marker.setContent(generateMarker(markerText));
}

function getStrokeColorByPollutionLevel(pollutionLevel) {
  switch (pollutionLevel) {
    case 0:
      return "#3cf24b";
    case 1:
      return "#e2ac2d";
    case 2:
      return "#f90000";
  }
}

function getFillColorByPollutionLevel(pollutionLevel) {
  switch (pollutionLevel) {
    case 0:
      return "#08e01a";
    case 1:
      return "#d3c60e";
    case 2:
      return "#a01818";
  }
}

function generateMarker(content) {
  return '<div class="richmarker-wrapper"><p>' + content + "</p></div>";
}

function createGuid() {
  return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
    (
      c ^
      (crypto.getRandomValues(new Uint8Array(1))[0] & (15 >> (c / 4)))
    ).toString(16)
  );
}

function getPropositionsByQuery(query) {
  return new Promise((resolve, reject) => {
    var request = {
      query: query,
      fields: ["formatted_address", "geometry", "id"],
      radius: 10000,
      location: config.map.options.center
    };
    placeService.textSearch(request, (result, status) => {
      if (status == google.maps.places.PlacesServiceStatus.OK) {
        resolve(
          result.map(f => {
            return {
              id: f.id,
              address: f.formatted_address,
              geometry: f.geometry
            };
          })
        );
        return;
      }
      reject();
    });
  });
}
function buildRoute(from, to) {
  directionsDisplay.setMap(map);
  var request = {
    origin: from,
    destination: to,
    travelMode: "WALKING"
  };
  directionsService.route(request, (response, status) => {
    if (status == google.maps.DirectionsStatus.OK) {
      directionsDisplay.setOptions({ preserveViewport: true });
      directionsDisplay.setDirections(response);
      //map.setZoom(config.map.options.zoom);
    }
  });
}

export default {
  initMap,
  createNewArea,
  updateArea,
  getPropositionsByQuery,
  buildRoute
};
