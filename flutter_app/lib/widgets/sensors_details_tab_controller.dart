import 'package:flutter/material.dart';

class SensorDetailsTabController extends DefaultTabController {
  SensorDetailsTabController()
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
                  children: [
                    Icon(Icons.directions_car),
                    Icon(Icons.directions_transit),
                  ],
                ))) {}
}
