import GoogleMapsLoader, { google } from "google-maps";
//import RichMarker from "js-rich-marker";
import { Observable } from "rxjs/Observable";
import { BehaviorSubject } from "rxjs";
import { Inject, Injectable } from "@angular/core";
import { Config } from "../../../models/providers/config";
import { PollutionLevels } from "../../../models/sensor.model";
import { MapProposition } from "../../../models/providers/map-proposition.model";
import { MapBuilder, AlertsService } from "../../inerfaces";
import { EventEmitter } from "events";
import { Geolocation } from "@ionic-native/geolocation";
import { EmulationModeTracker } from "./models/emulation.mode.tracker.model";
declare const google: any;
@Injectable()
export class GoogleMapBuilder implements MapBuilder {
  private map: google.maps.Map;
  private areas: Object;
  private autocompleteService: google.maps.places.AutocompleteService;
  private placeService: google.maps.places.PlacesService;
  private directionsService: google.maps.DirectionsService;
  private directionsDisplay: google.maps.DirectionsRenderer;
  private geocoder: google.maps.Geocoder;
  private eventEmitter: EventEmitter = new EventEmitter();
  private emulationModeTracker: EmulationModeTracker;
  constructor(public config: Config, private geolocation: Geolocation) {
    this.areas = {};
  }
  enableNavigationMode(): Promise<null> {
    return new Promise((resolve, reject) => {
      this.emulationModeTracker = new EmulationModeTracker();
      this.emulationModeTracker.userSelectionMarker = new google.maps.Marker({
        map: this.map,
        position: this.map.getCenter(),
        icon: {
          url:
            "https://cdn0.iconfinder.com/data/icons/small-n-flat/24/678111-map-marker-512.png", // url
          scaledSize: new google.maps.Size(50, 50), // scaled size
          origin: new google.maps.Point(0, 0), // origin
          anchor: new google.maps.Point(25, 25)
        },
        visible: false
      });
      this.emulationModeTracker.realUserPositionMarker = new google.maps.Marker(
        {
          icon: {
            url:
              "https://www.materialui.co/materialIcons/maps/my_location_grey_192x192.png", // url
            scaledSize: new google.maps.Size(25, 25), // scaled size
            origin: new google.maps.Point(0, 0), // origin
            anchor: new google.maps.Point(0, 0),
            labelOrigin: new google.maps.Point(10, -5) // anchor
          },
          map: null,
          position: this.map.getCenter(),
          label: "Вы"
        }
      );
      this.geolocation
        .getCurrentPosition()
        .then(resp => {
          this.map.setCenter(
            new google.maps.LatLng(resp.coords.latitude, resp.coords.longitude)
          );
          this.map.setZoom(20);
          this.emulationModeTracker.userSelectionMarker.setPosition(
            this.map.getCenter()
          );
          this.emulationModeTracker.userSelectionMarker.setMap(this.map);
          this.emulationModeTracker.realUserPositionMarker.setPosition(
            new google.maps.LatLng(resp.coords.latitude, resp.coords.longitude)
          );
          this.emulationModeTracker.realUserPositionMarker.setMap(this.map);
          resolve();
        })
        .catch(reject);
      this.emulationModeTracker.userPositionWatcher = this.geolocation
        .watchPosition()
        .subscribe(resp => {
          this.emulationModeTracker.realUserPositionMarker.setPosition(
            new google.maps.LatLng(resp.coords.latitude, resp.coords.longitude)
          );
        });
    });
  }

  navigationModeSetStartPostionMarkerByPointCoordinates(x: number, y: number) {
    if (this.emulationModeTracker.userSelectionMarker != null) {
      let point = new google.maps.Point(x, y);
      this.emulationModeTracker.userSelectionMarker.setPosition(
        this.point2LatLng(point, this.map)
      );
    }
  }

  isNavigationModeEnabled(): boolean {
    return this.emulationModeTracker != null;
  }

  disableNavigationMode(): Promise<null> {
    return new Promise((resolve, reject) => {
      try {
        this.map.setOptions(this.config.map.options);
        this.emulationModeTracker.realUserPositionMarker.setMap(null);
        this.emulationModeTracker.userPositionWatcher.unsubscribe();
        this.emulationModeTracker.userSelectionMarker.setMap(null);
        this.emulationModeTracker = null;
        resolve();
      } catch (ex) {
        reject(ex);
      }
    });
  }
  onStartPointSelected(handler: (...args: any[]) => void) {
    throw new Error("Method not implemented.");
  }
  public onDrag(handler: (...args: any[]) => void) {
    this.eventEmitter.on("dragstart", handler);
  }
  public onDragEnd(handler: (...args: any[]) => void) {
    this.eventEmitter.on("dragend", handler);
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
        this.map.addListener("dragstart", () => {
          this.eventEmitter.emit("dragstart");
        });
        this.map.addListener("dragend", () => {
          this.eventEmitter.emit("dragend");
        });
        resolve();
      });
      GoogleMapsLoader.onLoad(googleLib => {
        resolve();
      });
    });
  }
  public createNewArea(
    sensorId: number,
    pollutionLevel: PollutionLevels,
    lat: number,
    lng: number,
    markerText: string,
    onMarkerClick: Function
  ): void {
    this.areas[sensorId] = {};
    let position = new google.maps.LatLng(lat, lng);
    this.areas[sensorId].circle = new google.maps.Circle({
      strokeColor: this.getStrokeColorByPollutionLevel(pollutionLevel),
      strokeOpacity: 0.8,
      strokeWeight: 2,
      fillColor: this.getFillColorByPollutionLevel(pollutionLevel),
      fillOpacity: 0.35,
      map: this.map,
      center: position,
      radius: 1000
    });
    this.areas[sensorId].circle.addListener("click", onMarkerClick);
  }

  public updateArea(
    sensorId: number,
    pollutionLevel: PollutionLevels,
    markerText: string
  ): void {
    this.areas[sensorId].circle.setOptions({
      strokeColor: this.getStrokeColorByPollutionLevel(pollutionLevel),
      fillColor: this.getFillColorByPollutionLevel(pollutionLevel)
    });
    //this.areas[areaId].marker.setContent(this.generateMarker(markerText));
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
    return (([1e7] as any) + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
      (
        c ^
        (crypto.getRandomValues(new Uint8Array(1))[0] & (15 >> (c / 4)))
      ).toString(16)
    );
  }

  private latLng2Point(
    latLng: google.maps.LatLng,
    map: google.maps.Map
  ): google.maps.Point {
    var topRight = map
      .getProjection()
      .fromLatLngToPoint(map.getBounds().getNorthEast());
    var bottomLeft = map
      .getProjection()
      .fromLatLngToPoint(map.getBounds().getSouthWest());
    var scale = Math.pow(2, map.getZoom());
    var worldPoint = map.getProjection().fromLatLngToPoint(latLng);
    return new google.maps.Point(
      (worldPoint.x - bottomLeft.x) * scale,
      (worldPoint.y - topRight.y) * scale
    );
  }

  private point2LatLng(
    point: google.maps.Point,
    map: google.maps.Map
  ): google.maps.LatLng {
    var topRight = map
      .getProjection()
      .fromLatLngToPoint(map.getBounds().getNorthEast());
    var bottomLeft = map
      .getProjection()
      .fromLatLngToPoint(map.getBounds().getSouthWest());
    var scale = Math.pow(2, map.getZoom());
    var worldPoint = new google.maps.Point(
      point.x / scale + bottomLeft.x,
      point.y / scale + topRight.y
    );
    return map.getProjection().fromPointToLatLng(worldPoint);
  }
}
