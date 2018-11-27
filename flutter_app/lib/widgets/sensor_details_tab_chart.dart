/// Example of a simple line chart.
import 'package:charts_flutter/flutter.dart' as charts;
import 'package:flutter/material.dart';
import 'package:flutter_app/infrastructure/service_locator.dart';
import 'package:flutter_app/models/reading.dart';
import 'package:flutter_app/store/actions/update_sensor_action.dart';
import 'package:flutter_app/store/app_state.dart';
import 'package:redux/redux.dart';

class SensorDetailsTabChart extends StatefulWidget {
  num _sensorId;
  SensorDetailsTabChart(this._sensorId) {}
  @override
  _SensorDetailsTabChartState createState() =>
      _SensorDetailsTabChartState(_sensorId);
}

class _SensorDetailsTabChartState extends State<SensorDetailsTabChart>
    with AutomaticKeepAliveClientMixin<SensorDetailsTabChart> {
  @override
  // TODO: implement wantKeepAlive
  bool get wantKeepAlive => true;

  List<Reading> _readings = [];
  Store<AppState> _store;
  num _sensorId;

  _SensorDetailsTabChartState(this._sensorId) {
    _store = ServiceLocator.getService<Store<AppState>>();
  }

  @override
  initState() {
    super.initState();
    setState(() {
      _readings = _store.state.getSensorById(_sensorId).readings;
    });
    _store.onChange.listen((state) {
      if (_sensorId != null && state.lastAction is UpdateSensorAction) {
        var action = (state.lastAction as UpdateSensorAction);
        if (action.sensorId == _sensorId) {
          setState(() {
            _readings = state.getSensorById(_sensorId).readings;
          });
        }
      }
    });
  }

  @override
  Widget build(BuildContext context) {
    return new charts.LineChart(
      _getData(),
    );
  }

  /// Create one series with sample hard coded data.
  List<charts.Series<Reading, num>> _getData() {
    return [
      new charts.Series<Reading, num>(
        id: 'Sales',
        colorFn: (_, __) => charts.MaterialPalette.blue.shadeDefault,
        domainFn: (Reading reading, _) => reading.hum,
        measureFn: (Reading reading, _) => reading.dust,
        data: _readings,
      )
    ];
  }
}
