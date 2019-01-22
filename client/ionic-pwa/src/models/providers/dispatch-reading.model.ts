import { Reading } from "./../reading.model";
import { PollutionLevels } from "../sensor.model";
export class HubDispatchModel {
  sensorId: number;
  reading: Reading;
  latestPollutionLevel: PollutionLevels;
}
