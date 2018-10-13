import { Observable } from "rxjs/Observable";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Sensor } from "../../../models/sensor.model";
import { IApiProvider } from "../../inerfaces/api/api.provider";

/**
 * Api is a generic REST Api handler. Set your API url first.
 */
@Injectable()
export class ApiProvider implements IApiProvider {
  url: string = "https://example.com/api/v1";

  constructor(public http: HttpClient) {}

  getAllSensors(): Observable<Sensor[]> {
    return this.http.get<Sensor[]>(this.url + "api/sensors");
  }
}
