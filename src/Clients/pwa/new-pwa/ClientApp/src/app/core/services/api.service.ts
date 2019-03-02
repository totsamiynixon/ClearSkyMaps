import { Injectable, Inject } from "@angular/core";
import { Observable } from "rxjs";
import { Sensor } from "../models/sensor.model";
import { IConfig } from "../models/config.model";
import { HttpClient } from '@angular/common/http';
import { APP_CONFIG } from "../config.fabric";

@Injectable({
    providedIn: 'root',
})
export class ApiService {
    constructor(public http: HttpClient, @Inject(APP_CONFIG) private config: IConfig) { }

    getAllSensors(): Observable<Sensor[]> {
        return this.http.get<Sensor[]>(
            this.config.applicationServerUrl + "api/sensors"
        );
    }
}