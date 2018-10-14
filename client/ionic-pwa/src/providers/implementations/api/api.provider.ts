import { Observable } from "rxjs/Observable";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Sensor } from "../../../models/sensor.model";
import { ApiProvider } from "../../inerfaces/api/api.provider";
import { Config } from "../../../models/providers/config";

/**
 * Api is a generic REST Api handler. Set your API url first.
 */
@Injectable()
export class DefaultApiProvider implements ApiProvider {
  constructor(public http: HttpClient, private config: Config) {}

  getAllSensors(): Observable<Sensor[]> {
    return this.http.get<Sensor[]>(
      this.config.applicationServerUrl + "/api/sensors"
    );
  }
}
