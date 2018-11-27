import 'package:flutter/material.dart';
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
                      Tab(icon: Icon(Icons.directions_car)),
                      Tab(icon: Icon(Icons.directions_transit)),
                    ],
                  ),
                  title: Text('Tabs Demo'),
                ),
                body: TabBarView(
                  physics: new NeverScrollableScrollPhysics(),
                  children: [
                    SensorDetailsTabTable(_sensorId),
                    Icon(Icons.directions_transit),
                  ],
                ))) {}
}
