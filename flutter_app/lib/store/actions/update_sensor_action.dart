import 'package:flutter_app/models/enum/pollution_levels_enum.dart';
import 'package:flutter_app/models/reading.dart';

class UpdateSensorAction {
  final Reading payload;
  final int sensorId;
  final PollutionLevels pollutionLevel;
  UpdateSensorAction(this.payload, this.sensorId, this.pollutionLevel) {}
}
