import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Sensor } from "../models/sensor.model";
import { IConfig } from "../models/config.model";
import { HttpClient } from '@angular/common/http';

@Injectable({
    providedIn: 'root',
})
export class ApiService {
    constructor(public http: HttpClient, private config: IConfig) { }

    getAllSensors(): Observable<Sensor[]> {
        return this.http.get<Sensor[]>(
            this.config.applicationServerUrl + "api/sensors"
        );
    }
}