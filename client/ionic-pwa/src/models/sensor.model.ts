import { Reading } from "./reading.model";

export class Sensor {
  id: number;
  latitude: number;
  longitude: number;
  readings: Reading[];
  latestPollutionLevel: PollutionLevels;
}

export enum PollutionLevels {
  Low = 1,
  Medium = 2,
  High = 3
}
