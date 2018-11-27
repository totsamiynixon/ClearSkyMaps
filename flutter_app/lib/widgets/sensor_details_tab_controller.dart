import 'package:flutter/material.dart';
import 'package:flutter_app/widgets/sensor_details_tab_chart.dart';
import 'package:flutter_app/widgets/sensor_details_tab_table.dart';

class SensorDetailsTabController extends DefaultTabController {
  num _sensorId;
  SensorDetailsTabController(this._sensorId)
      : super(
            length: 2,
            child: Scaffold(
                appBar: AppBar(
                  bottom: TabBar(
                    tabs: [
                      Tab(icon: Icon(Icons.table_chart)),
                      Tab(icon: Icon(Icons.show_chart)),
                    ],
                  ),
                  title: Text('Tabs Demo'),
                ),
                body: TabBarView(
                  physics: new NeverScrollableScrollPhysics(),
                  children: [
                    SensorDetailsTabTable(_sensorId),
                    SensorDetailsTabChart(_sensorId),
                  ],
                ))) {}
}
