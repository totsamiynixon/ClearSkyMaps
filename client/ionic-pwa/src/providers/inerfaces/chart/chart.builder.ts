import { Sensor } from "../../../models/sensor.model";
import { Parameters } from "../../../models/parameters.enum";

export interface IChartBuilder {
  initChart(element: HTMLCanvasElement): void;
  updateDataset(currentSensor: Sensor, currentParameter: Parameters): void;
}
