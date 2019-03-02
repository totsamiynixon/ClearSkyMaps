import { google } from "google-maps";
import GoogleMapsLoader = require('google-maps');
import { Injectable, Inject } from "@angular/core";
import { IConfig } from "../models/config.model";
import { PollutionLevel } from "../models/pollutionLevel.enum";
import { APP_CONFIG } from "../config.fabric";
declare const google: any;
@Injectable({
  providedIn: "root"
})
export class MapService {
  private map: any;
  private areas: Object;
  constructor(
    @Inject(APP_CONFIG) private config: IConfig
  ) {
    this.areas = {};
    GoogleMapsLoader.KEY = this.config.map.key;
    GoogleMapsLoader.LIBRARIES = ["geometry", "places"];
    GoogleMapsLoader.LANGUAGE = "ru";
  }
  public initMap(element: HTMLElement): Promise<void> {
    return new Promise((resolve, reject) => {
      if (this.map != null) {
        GoogleMapsLoader.release(() => {
          this.map = null;
          this.areas = {};
        });
      }
      GoogleMapsLoader.load(google => {
        this.map = new google.maps.Map(element, this.config.map.options);
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
