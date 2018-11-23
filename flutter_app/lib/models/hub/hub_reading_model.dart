import 'package:flutter_app/models/enum/pollution_levels_enum.dart';
import 'package:flutter_app/models/reading.dart';

class HubDispatchModel {
  int sensorId;

  PollutionLevels latestPollutionLevel;

  Reading reading;
}
