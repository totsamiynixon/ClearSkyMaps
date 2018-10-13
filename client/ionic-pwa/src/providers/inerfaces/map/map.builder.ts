import { MapProposition } from "../../../models/providers/map-proposition.model";
import { PollutionLevels } from "../../../models/sensor.model";

export interface IMapBuilder {
  buildRoute(
    from: google.maps.LatLng | google.maps.LatLngLiteral,
    to: google.maps.LatLng | google.maps.LatLngLiteral
  ): void;
  getPropositionsByQuery(query: string): Promise<Array<MapProposition>>;
  updateArea(
    areaId: string,
    pollutionLevel: PollutionLevels,
    markerText: string
  ): void;
  createNewArea(
    pollutionLevel: PollutionLevels,
    lat: number,
    lng: number,
    markerText: string,
    onMarkerClick: Function
  ): string;
  initMap(element: HTMLElement): Promise<void>;
}
