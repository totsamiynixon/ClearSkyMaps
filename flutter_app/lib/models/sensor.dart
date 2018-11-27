import 'dart:convert';

import 'package:flutter_app/models/enum/pollution_levels_enum.dart';
import 'package:flutter_app/models/reading.dart';

class Sensor {
  int id;
  double latitude;
  double longitude;
  List<Reading> readings;
  PollutionLevels latestPollutionLevel;

  Sensor();

  factory Sensor.fromJson(Map<String, dynamic> data) {
    var sensor = Sensor();
    sensor.id = data['id'];
    sensor.latitude = data['latitude'];
    sensor.longitude = data['longitude'];
    sensor.latestPollutionLevel =
        PollutionLevels.values[data['latestPollutionLevel']];
    sensor.readings = (data['readings'] as List<dynamic>)
        .map((e) => new Reading.fromJson(e))
        .toList();
    return sensor;
  }
}
