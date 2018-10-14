import { Observable } from "rxjs/Observable";
import { Sensor } from "../../../models/sensor.model";
import { Injectable } from "@angular/core";

@Injectable()
export abstract class ApiProvider {
  abstract getAllSensors(): Observable<Sensor[]>;
}
