import { Action } from "@ngrx/store";
import { Parameters } from "../../../models/parameters.enum";
import { Sensor } from "../../../models/sensor.model";

export interface IHomePageState {
  filterByParameter: Parameters;
  sensors: Sensor[];
  lastAction: Action;
}
