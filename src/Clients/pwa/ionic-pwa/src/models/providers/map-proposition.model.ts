import { google } from "google-maps";
export class MapProposition {
  id: string;
  address: string;
  location: google.maps.LatLng | google.maps.LatLngBoundsLiteral;
}
