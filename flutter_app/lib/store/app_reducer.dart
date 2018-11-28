import 'package:flutter_app/store/actions/set_sensors_action.dart';
import 'package:flutter_app/store/actions/update_sensor_action.dart';
import 'package:flutter_app/store/app_state.dart';

class AppReducer {
  static AppState Execute(AppState state, dynamic action) {
    state.lastAction = action;
    if (action is SetSensorsAction) {
      state.sensors = (action as SetSensorsAction).payload;
      return state;
    }
    if (action is UpdateSensorAction) {
      var updateAction = action as UpdateSensorAction;
      var updateSensor =
          state.sensors.firstWhere((f) => f.id == updateAction.sensorId);
      if (updateSensor == null) {
        return state;
      }
      updateSensor.latestPollutionLevel = updateAction.pollutionLevel;
      updateSensor.readings.add(updateAction.payload);
      updateSensor.readings.sort((a, b) =>
          (b.created.difference(a.created).inMilliseconds) > 0 ? 1 : 0);
      updateSensor.readings = updateSensor.readings.take(10).toList();
      return state;
    }
    return state;
  }
}