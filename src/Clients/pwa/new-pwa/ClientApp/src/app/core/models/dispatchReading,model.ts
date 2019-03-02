import { PollutionLevel } from "./pollutionLevel.enum";
import { Reading } from "./reading.model";

export class HubDispatchModel {
  sensorId: number;
  reading: Reading;
  latestPollutionLevel: PollutionLevel;
}
