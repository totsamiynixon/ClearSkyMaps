import { Sensor } from "../../../models/sensor.model";
import { Parameters } from "../../../models/parameters.enum";
import { Injectable } from "@angular/core";
@Injectable()
export abstract class ChartBuilder {
  abstract initChart(element: HTMLCanvasElement): void;
  abstract updateDataset(
    currentSensor: Sensor,
    currentParameter: Parameters
  ): void;
}
