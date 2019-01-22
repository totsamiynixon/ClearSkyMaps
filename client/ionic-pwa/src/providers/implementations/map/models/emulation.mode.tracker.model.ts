import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs";
import * as GeolocationMarker from "geolocation-marker";
export class EmulationModeTracker {
  startRouteMarker: google.maps.Marker;
  endRouteMarker: google.maps.Marker;
  userPositionWatcher: Subscription;
  userPositionMarker: google.maps.Marker;
}
