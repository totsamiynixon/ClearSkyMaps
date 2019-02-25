import { Parameters } from "../../../models/parameters.enum";
import { Sensor } from "../../../models/sensor.model";

export class TabModel {
  sensor: Sensor;
  filterByParameter: Parameters;
}
