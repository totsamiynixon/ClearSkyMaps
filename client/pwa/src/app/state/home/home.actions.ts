import { Action } from "@ngrx/store";
import { Sensor } from "src/app/core/models/sensor.model";
import { Reading } from "src/app/core/models/reading.model";
import { PollutionLevel } from "src/app/core/models/pollutionLevel.enum";

export enum HomePageActionTypes {
  INIT_MAP = "[HOME] Initialize Map",
  SET_SENSORS = "[HOME] Set Sensors",
  SET_SENSORS_SUCCESS = "[HOME] Set Sensors Success",
  UPDATE_SENSOR = "[HOME] Update Sensor"
}

export class InitMap implements Action {
  readonly type = HomePageActionTypes.INIT_MAP;
  constructor(public element: HTMLElement) { }
}


export class SetSensors implements Action {
  readonly type = HomePageActionTypes.SET_SENSORS;
  constructor() { }
}

export class SetSensorsSuccess implements Action {
  readonly type = HomePageActionTypes.SET_SENSORS_SUCCESS;
  constructor(public payload: Sensor[]) { }
}

export class UpdateSensor implements Action {
  readonly type = HomePageActionTypes.UPDATE_SENSOR;
  constructor(
    public payload: Reading,
    public sensorId: number,
    public pollutionLevel: PollutionLevel
  ) { }
}

export type HomePageActions = SetSensors | SetSensorsSuccess | UpdateSensor | InitMap;
