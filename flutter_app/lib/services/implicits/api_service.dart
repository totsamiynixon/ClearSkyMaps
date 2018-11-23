import 'package:flutter_app/models/sensor.dart';

abstract class ApiClientService {
  Future<List<Sensor>> getSensorsAsync();
}
