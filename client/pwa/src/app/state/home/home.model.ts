import { Sensor } from "src/app/core/models/sensor.model";
import { Action } from "@ngrx/store";

export interface IHomePageState {
    sensors: Sensor[];
  }
  