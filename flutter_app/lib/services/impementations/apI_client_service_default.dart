import 'dart:convert';

import 'package:flutter_app/config/app_config.dart';
import 'package:flutter_app/models/sensor.dart';
import 'package:http/http.dart' as http;
import 'package:flutter_app/services/implicits/api_service.dart';

class ApiClientServiceDefault extends ApiClientService {
  final AppConfig config;
  ApiClientServiceDefault(this.config) {}

  Future<List<Sensor>> getSensorsAsync() async {
    var response = await http.get("${config.baseServiceUrl}/api/sensors");
    if (response.statusCode == 200) {
      return (json.decode(response.body) as List)
          .map((e) => new Sensor.fromJson(e))
          .toList();
    }
    return null;
  }
}
