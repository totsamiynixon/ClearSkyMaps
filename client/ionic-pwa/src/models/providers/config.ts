import { Injectable } from "@angular/core";
import { google } from "google-maps";
@Injectable()
export class Config {
  hubPath: string;
  applicationServerUrl: string;
  map: ConfigMap;
}

export class ConfigMap {
  key: string;
  options: google.maps.MapOptions;
}
