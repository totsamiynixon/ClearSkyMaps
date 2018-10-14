import { MapProposition } from "../../../models/providers/map-proposition.model";
import { PollutionLevels } from "../../../models/sensor.model";
import { Observable } from "rxjs/Observable";
import { Injectable } from "@angular/core";
@Injectable()
export abstract class MapBuilder {
  abstract buildRoute(
    from: google.maps.LatLng | google.maps.LatLngLiteral,
    to: google.maps.LatLng | google.maps.LatLngLiteral
  ): void;
  abstract getPropositionsByQuery(
    query: string
  ): Promise<Array<MapProposition>>;
  abstract updateArea(
    sensorId: number,
    pollutionLevel: PollutionLevels,
    markerText: string
  ): void;
  abstract createNewArea(
    sensorId: number,
    pollutionLevel: PollutionLevels,
    lat: number,
    lng: number,
    markerText: string,
    onMarkerClick: Function
  ): void;
  abstract initMap(element: HTMLElement): Promise<void>;
  abstract onDrag(handler: (...args: any[]) => void);
  abstract onDragEnd(handler: (...args: any[]) => void);
}
