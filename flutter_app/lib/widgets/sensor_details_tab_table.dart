import 'dart:async';

import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_app/infrastructure/service_locator.dart';
import 'package:flutter_app/models/reading.dart';
import 'package:flutter_app/store/actions/update_sensor_action.dart';
import 'package:flutter_app/store/app_state.dart';
import 'package:intl/intl.dart';
import 'package:redux/redux.dart';

class SensorDetailsTabTable extends StatefulWidget {
  num _sensorId;
  SensorDetailsTabTable(this._sensorId) {}
  @override
  _SensorDetailsTabTableState createState() =>
      _SensorDetailsTabTableState(_sensorId);
}

class _SensorDetailsTabTableState extends State<SensorDetailsTabTable>
    with AutomaticKeepAliveClientMixin<SensorDetailsTabTable> {
  List<Reading> _readings = [];
  Store<AppState> _store;
  num _sensorId;
  StreamSubscription<AppState> _stateSubscription;

  _SensorDetailsTabTableState(this._sensorId) {
    _store = ServiceLocator.getService<Store<AppState>>();
  }

  @override
  initState() {
    super.initState();
    setState(() {
      _readings = _store.state.getSensorById(_sensorId).readings;
    });
    _stateSubscription = _store.onChange.listen(_handleStateChange);
  }

  @protected
  @mustCallSuper
  void deactivate() {
    super.deactivate();
    _stateSubscription.cancel();
  }

  void _handleStateChange(AppState state) {
    if (_sensorId != null && state.lastAction is UpdateSensorAction) {
      var action = (state.lastAction as UpdateSensorAction);
      if (action.sensorId == _sensorId) {
        setState(() {
          _readings = state.getSensorById(_sensorId).readings;
        });
      }
    }
  }

  List<DataRow> _getRows() {
    if (_readings == null) {
      return [];
    }
    return _readings
        .map((reading) => DataRow(cells: <DataCell>[
              DataCell(Text('${DateFormat('Hms').format(DateTime.now())}')),
              DataCell(Text('${reading.cO2}')),
              DataCell(Text('${reading.cO2}')),
              DataCell(Text('${reading.cO}')),
              DataCell(Text('${reading.cH4}')),
              DataCell(Text('${reading.dust}')),
              DataCell(Text('${reading.temp}')),
              DataCell(Text('${reading.hum}')),
              DataCell(Text('${reading.preassure}'))
            ]))
        .toList();
  }

  @override
  Widget build(BuildContext context) {
    //Bug with Data Table and Keep Alive! Don't forget to report!
    return Container(
        child: ListView(
            padding: const EdgeInsets.all(15.0),
            scrollDirection: Axis.horizontal,
            children: <Widget>[
          DataTable(columns: <DataColumn>[
            DataColumn(
              label: const Text('Снято'),
            ),
            DataColumn(
              label: const Text('CO2'),
              numeric: true,
            ),
            DataColumn(
              label: const Text('LPG'),
              numeric: true,
            ),
            DataColumn(label: const Text('CO'), numeric: true),
            DataColumn(
              label: const Text('CH4'),
              numeric: true,
            ),
            DataColumn(
              label: const Text('Пыль'),
              numeric: true,
            ),
            DataColumn(
              label: const Text('Т-ра'),
              numeric: true,
            ),
            DataColumn(
              label: const Text('Hum'),
              numeric: true,
            ),
            DataColumn(
              label: const Text('P'),
              numeric: true,
            )
          ], rows: _getRows())
        ]));
  }

  @override
  // TODO: implement wantKeepAlive
  bool get wantKeepAlive => true;
}
