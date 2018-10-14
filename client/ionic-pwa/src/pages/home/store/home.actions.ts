import { Action } from "@ngrx/store";
import { Parameters } from "../../../models/parameters.enum";
import { Sensor, PollutionLevels } from "../../../models/sensor.model";
import { Reading } from "../../../models/reading.model";

export const SET_FILTER_PARAMETER: string = "SET_FILTER_PARAMETER";

export class SetFilterParameterAction implements Action {
  readonly type = SET_FILTER_PARAMETER;
  constructor(public payload: Parameters) {}
}

export const SET_SENSORS: string = "SET_SENSORS";

export class SetSensorsAction implements Action {
  readonly type = SET_SENSORS;
  constructor(public payload: Sensor[]) {}
}

export const UPDATE_SENSOR: string = "UPDATE_SENSOR";

export class UpdateSensorAction implements Action {
  readonly type = UPDATE_SENSOR;
  constructor(
    public payload: Reading,
    public sensorId: number,
    public pollutionLevel: PollutionLevels
  ) {}
}
