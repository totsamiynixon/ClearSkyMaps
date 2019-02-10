import GoogleMapsLoader, { google } from "google-maps";
import { Injectable, Inject } from "@angular/core";
import { EventEmitter } from "events";
import { IConfig } from "../models/config.model";
import { PollutionLevel } from "../models/pollutionLevel.enum";
import { HomePageModule } from "src/app/pages/home/home.module";
import { APP_CONFIG } from "src/app/app.module";
declare const google: any;
@Injectable({
  providedIn: HomePageModule
})
export class MapService {
  private map: any;
  private areas: Object;
  private directionsDisplay: google.maps.DirectionsRenderer;
  private eventEmitter: EventEmitter = new EventEmitter();
  constructor(
    @Inject(APP_CONFIG) private config: IConfig
  ) {
    this.areas = {};
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
        this.directionsDisplay = new google.maps.DirectionsRenderer();
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
    pollutionLevel: PollutionLevel,
    lat: number,
    lng: number,
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
    pollutionLevel: PollutionLevel,
  ): void {
    this.areas[sensorId].circle.setOptions({
      strokeColor: this.getStrokeColorByPollutionLevel(pollutionLevel),
      fillColor: this.getFillColorByPollutionLevel(pollutionLevel)
    });
  }

  private getStrokeColorByPollutionLevel(
    pollutionLevel: PollutionLevel
  ): string {
    switch (pollutionLevel) {
      case PollutionLevel.Low:
        return "#3cf24b";
      case PollutionLevel.Medium:
        return "#e2ac2d";
      case PollutionLevel.High:
        return "#f90000";
    }
  }

  private getFillColorByPollutionLevel(
    pollutionLevel: PollutionLevel
  ): string {
    switch (pollutionLevel) {
      case PollutionLevel.Low:
        return "#08e01a";
      case PollutionLevel.Medium:
        return "#d3c60e";
      case PollutionLevel.High:
        return "#a01818";
    }
  }
}
