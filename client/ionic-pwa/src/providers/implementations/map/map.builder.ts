import GoogleMapsLoader, { google } from "google-maps";
//import RichMarker from "js-rich-marker";
import { Observable } from "rxjs/Observable";
import { BehaviorSubject } from "rxjs";
import { Inject } from "@angular/core";
import { Config } from "../../../models/providers/config";
import { PollutionLevels } from "../../../models/sensor.model";
import { MapProposition } from "../../../models/providers/map-proposition.model";
import { IMapBuilder } from "../../inerfaces/map/map.builder";
export class MapBuilder implements IMapBuilder {
  private map: google.maps.Map;
  private areas: Object;
  private autocompleteService: google.maps.places.AutocompleteService;
  private placeService: google.maps.places.PlacesService;
  private directionsService: google.maps.DirectionsService;
  private directionsDisplay: google.maps.DirectionsRenderer;
  private geocoder: google.maps.Geocoder;
  private dragEmitter: BehaviorSubject<string>;
  constructor(@Inject(Config) public config: Config) {
    this.areas = {};
    this.dragEmitter = new BehaviorSubject<string>(null);
  }
  public initMap(element: HTMLElement): Promise<void> {
    return new Promise((resolve, reject) => {
      GoogleMapsLoader.release(() => {
        this.map = null;
        this.areas = {};
      });
      GoogleMapsLoader.KEY = this.config.map.key;
      GoogleMapsLoader.LIBRARIES = ["geometry", "places"];
      GoogleMapsLoader.LANGUAGE = "ru";
      GoogleMapsLoader.load(google => {
        this.map = new google.maps.Map(element, this.config.map.options);
        this.autocompleteService = new google.maps.places.AutocompleteService();
        this.placeService = new google.maps.places.PlacesService(this.map);
        this.directionsService = new google.maps.DirectionsService();
        this.directionsDisplay = new google.maps.DirectionsRenderer();
        this.geocoder = new google.maps.Geocoder();
        this.map.addListener("drag", () => {
          this.dragEmitter.next("drag");
        });
        this.map.addListener("dragend", () => {
          this.dragEmitter.next("dragend");
        });
        resolve();
      });
      GoogleMapsLoader.onLoad(googleLib => {
        resolve();
      });
    });
  }
  public createNewArea(
    pollutionLevel: PollutionLevels,
    lat: number,
    lng: number,
    markerText: string,
    onMarkerClick: Function
  ) {
    let guid = this.createGuid();
    this.areas[guid] = {};
    let position = new google.maps.LatLng(lat, lng);
    this.areas[guid].circle = new google.maps.Circle({
      strokeColor: this.getStrokeColorByPollutionLevel(pollutionLevel),
      strokeOpacity: 0.8,
      strokeWeight: 2,
      fillColor: this.getFillColorByPollutionLevel(pollutionLevel),
      fillOpacity: 0.35,
      map: this.map,
      center: position,
      radius: 1000
    });
    // this.areas[guid].marker = new RichMarker({
    //   position: position,
    //   map: this.map,
    //   content: this.generateMarker(markerText),
    //   shadow: "none"
    // });
    // this.areas[guid].marker.setOptions({ opacity: 0.8 });
    // this.areas[guid].marker.addListener("click", onMarkerClick);
    return guid;
  }

  public updateArea(
    areaId: string,
    pollutionLevel: PollutionLevels,
    markerText: string
  ): void {
    this.areas[areaId].circle.setOptions({
      strokeColor: this.getStrokeColorByPollutionLevel(pollutionLevel),
      fillColor: this.getFillColorByPollutionLevel(pollutionLevel)
    });
    this.areas[areaId].marker.setContent(this.generateMarker(markerText));
  }
  public getPropositionsByQuery(query: string): Promise<Array<MapProposition>> {
    return new Promise((resolve, reject) => {
      let request: google.maps.places.TextSearchRequest = {
        query: query,
        radius: 10000,
        location: this.config.map.options.center
      };
      this.placeService.textSearch(request, (result, status) => {
        if (status == google.maps.places.PlacesServiceStatus.OK) {
          resolve(
            result.map(f => {
              return {
                id: f.id,
                address: f.formatted_address,
                location: f.geometry.location
              } as MapProposition;
            })
          );
          return;
        }
        reject();
      });
    });
  }
  public buildRoute(
    from: google.maps.LatLng | google.maps.LatLngLiteral,
    to: google.maps.LatLng | google.maps.LatLngLiteral
  ): void {
    this.directionsDisplay.setMap(this.map);
    var abc = {
      origin: from,
      destination: to,
      travelMode: google.maps.TravelMode.WALKING
    };
    let request: google.maps.DirectionsRequest = {
      origin: from,
      destination: to,
      travelMode: google.maps.TravelMode.WALKING
    };
    this.directionsService.route(request, (response, status) => {
      if (status == google.maps.DirectionsStatus.OK) {
        this.directionsDisplay.setDirections(response);
      }
    });
  }

  public drag(): Observable<string> {
    return this.dragEmitter;
  }
  private getStrokeColorByPollutionLevel(
    pollutionLevel: PollutionLevels
  ): string {
    switch (pollutionLevel) {
      case PollutionLevels.Low:
        return "#3cf24b";
      case PollutionLevels.Medium:
        return "#e2ac2d";
      case PollutionLevels.High:
        return "#f90000";
    }
  }

  private getFillColorByPollutionLevel(
    pollutionLevel: PollutionLevels
  ): string {
    switch (pollutionLevel) {
      case PollutionLevels.Low:
        return "#08e01a";
      case PollutionLevels.Medium:
        return "#d3c60e";
      case PollutionLevels.High:
        return "#a01818";
    }
  }

  private generateMarker(content: string): string {
    return '<div class="richmarker-wrapper"><p>' + content + "</p></div>";
  }

  private createGuid(): string {
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
      (
        c ^
        (crypto.getRandomValues(new Uint8Array(1))[0] & (15 >> (c / 4)))
      ).toString(16)
    );
  }
}
