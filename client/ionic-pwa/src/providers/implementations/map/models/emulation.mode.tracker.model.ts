import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs";
import * as GeolocationMarker from "geolocation-marker";
export class EmulationModeTracker {
  userSelectionMarker: google.maps.Marker;
  userPositionWatcher: Subscription;
  realUserPositionMarker: google.maps.Marker;
}
