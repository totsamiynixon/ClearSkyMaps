import { Reading } from "./reading.model";
import { PollutionLevel } from "./pollutionLevel.enum";

export class Sensor {
  id: number;
  latitude: number;
  longitude: number;
  readings: Reading[];
  latestPollutionLevel: PollutionLevel;
}
