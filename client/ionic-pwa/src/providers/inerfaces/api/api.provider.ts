import { Observable } from "rxjs/Observable";
import { Sensor } from "../../../models/sensor.model";

export interface IApiProvider {
  getAllSensors(): Observable<Sensor[]>;
}
