import 'package:flutter_app/models/enum/pollution_levels_enum.dart';
import 'package:flutter_app/models/reading.dart';

class Sensor {
  int id;
  double latitude;
  double longitude;
  List<Reading> readings;
  PollutionLevels latestPollutionLevel;
}
