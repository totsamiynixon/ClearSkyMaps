import 'package:flutter_app/models/sensor.dart';

class AppState {
  List<Sensor> sensors;
  Object lastAction;
  Sensor getSensorById(int sensorId) {
    var result = sensors.firstWhere((s) => s.id == sensorId, orElse: null);
    return result;
  }
}
