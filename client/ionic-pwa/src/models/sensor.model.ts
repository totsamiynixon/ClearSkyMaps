import { Reading } from "./reading.model";

export class Sensor {
  id: number;
  latitude: number;
  longitude: number;
  readings: Reading[];
  latestPollutionLevel: PollutionLevels;
}

export enum PollutionLevels {
  Low = 0,
  Medium = 1,
  High = 2
}
