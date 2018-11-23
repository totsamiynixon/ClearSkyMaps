import 'package:flutter_app/models/sensor.dart';

class SetSensorsAction {
  final List<Sensor> payload;
  SetSensorsAction(this.payload) {}
}
