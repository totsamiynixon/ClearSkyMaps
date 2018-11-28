import 'package:flutter_app/models/enum/pollution_levels_enum.dart';
import 'package:flutter_app/models/reading.dart';

class HubDispatchModel {
  HubDispatchModel() {}
  int sensorId;

  PollutionLevels latestPollutionLevel;

  Reading reading;

  factory HubDispatchModel.fromJson(Map<String, dynamic> data) {
    var model = new HubDispatchModel();
    model.sensorId = data['sensorId'];
    model.latestPollutionLevel =
        PollutionLevels.values[data['latestPollutionLevel']];
    model.reading = Reading.fromJson(data['reading']);
    return model;
  }
}
