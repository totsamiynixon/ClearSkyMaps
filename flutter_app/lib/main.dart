import 'package:flutter/material.dart';
import 'package:flutter_app/bootstrapper.dart';
import 'package:flutter_app/widgets/sensor_details_tab_controller.dart';
import 'package:flutter_app/widgets/sensors_map_page.dart';

void main() {
  Bootstrapper.run();
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  // This widget is the root of your application.F
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
      theme: ThemeData(primarySwatch: Colors.teal),
      routes: <String, WidgetBuilder>{
        '/': (BuildContext context) =>
            SensorsMap(), //MapsDemo(_mapWidget, _mapController)
      },
    );
  }
}
