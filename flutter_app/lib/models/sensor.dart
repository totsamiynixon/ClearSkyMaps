import 'package:flutter_app/models/enum/pollution_levels_enum.dart';
import 'package:flutter_app/models/reading.dart';

class Sensor {
  int id;
  double latitude;
  double longitude;
  List<Reading> readings;
  PollutionLevels latestPollutionLevel;

  Sensor();

  factory Sensor.fromJson(Map<String, dynamic> json) {
    var sensor = Sensor();
    sensor.id = json['id'];
    sensor.latitude = json['latitude'];
    sensor.longitude = json['longitude'];
    sensor.latestPollutionLevel =
        PollutionLevels.values[json['latestPollutionLevel']];
    return sensor;
  }
}
