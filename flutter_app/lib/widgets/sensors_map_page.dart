import 'package:flutter/material.dart';
import 'package:flutter_app/infrastructure/service_locator.dart';
import 'package:flutter_app/models/sensor.dart';
import 'package:flutter_app/services/implicits/api_service.dart';
import 'package:flutter_app/store/actions/set_sensors_action.dart';
import 'package:flutter_app/store/app_state.dart';
import 'package:flutter_app/widgets/sensor_details_tab_controller.dart';
import 'package:flutter_map/flutter_map.dart';
import 'package:latlong/latlong.dart';
import 'package:redux/redux.dart';

class _SensorsMapState extends State<SensorsMap> {
  ApiClientService _apiClient;
  Store<AppState> _store;
  List<Sensor> _sensors = List<Sensor>();
  _SensorsMapState() {
    _apiClient = ServiceLocator.getService<ApiClientService>();
    _store = ServiceLocator.getService<Store<AppState>>();
  }

  @override
  initState() {
    super.initState();
    _store.onChange.listen((state) {
      if (state.lastAction is SetSensorsAction) {
        var action = state.lastAction as SetSensorsAction;
        if (this.mounted)
          setState(() {
            _sensors = action.payload;
          });
      }
    });
    _apiClient.getSensorsAsync().then((sensors) {
      _store.dispatch(SetSensorsAction(sensors));
    });
  }

  Drawer getDrawer(BuildContext context) {
    return Drawer(
        // Add a ListView to the drawer. This ensures the user can scroll
        // through the options in the Drawer if there isn't enough vertical
        // space to fit everything.
        child: ListView(
      // Important: Remove any padding from the ListView.
      padding: EdgeInsets.zero,
      children: <Widget>[
        DrawerHeader(
          child: Text('Drawer Header', style: TextStyle(color: Colors.white)),
          decoration: BoxDecoration(
            color: Colors.teal,
          ),
        ),
        ListTile(
          title: Text('Item 1'),
          onTap: () {
            // Update the state of the app
            // ...
            // Then close the drawer
            Navigator.pop(context);
          },
        ),
        ListTile(
          title: Text('Item 2'),
          onTap: () {
            // Update the state of the app
            // ...
            // Then close the drawer
            Navigator.pop(context);
          },
        ),
      ],
    ));
  }

  Widget getBody() {
    //return Center(child: mapWidget);
    return FlutterMap(
      options: new MapOptions(
        center: new LatLng(53.902149, 27.538465),
        zoom: 13.0,
      ),
      layers: [
        new TileLayerOptions(
          urlTemplate: "https://api.tiles.mapbox.com/v4/"
              "{id}/{z}/{x}/{y}@2x.png?access_token={accessToken}",
          additionalOptions: {
            'accessToken':
                'pk.eyJ1IjoidG90c2FtaXluaXhvbiIsImEiOiJjam9ydHkyZHkwamtsM3VwdDJkNXFtcG81In0.cT2GmhMwDVVCXgGJicbSQA',
            'id': 'mapbox.streets',
          },
        ),
        new MarkerLayerOptions(
          markers: this
              ._sensors
              .map((sensor) => Marker(
                  point: new LatLng(sensor.latitude, sensor.longitude),
                  builder: (ctx) => new Container(
                        child: InkWell(
                          child: Icon(Icons.directions_transit),
                          onTap: () {
                            //Navigator.pushNamed(ctx, '/details');
                            var route = new MaterialPageRoute(
                              builder: (BuildContext context) =>
                                  new SensorDetailsTabController(sensor.id),
                            );
                            Navigator.of(context).push(route);
                          },
                        ),
                      )))
              .toList(),
        ),
      ],
    );
  }

  AppBar getAppBar(BuildContext context) {
    return AppBar(
      title: Text("Sensors Map"),
      actions: <Widget>[
        IconButton(
          icon: Icon(Icons.add),
          onPressed: () async {
            Navigator.pushNamed(context, "/details");
            // var sensors = await _apiClient.getSensorsAsync();
            // _showDialog(context, sensors.length.toString());
          },
        )
      ],
    );
  }

  void _showDialog(BuildContext context, String info) {
    // flutter defined function
    showDialog(
      context: context,
      builder: (BuildContext context) {
        // return object of type Dialog
        return AlertDialog(
          title: new Text("Alert Dialog title"),
          content: new Text(info),
          actions: <Widget>[
            // usually buttons at the bottom of the dialog
            new FlatButton(
              child: new Text("Close"),
              onPressed: () {
                Navigator.of(context).pop();
              },
            ),
          ],
        );
      },
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: getAppBar(context),
        body: getBody(),
        drawer: getDrawer(context));
  }
}

class SensorsMap extends StatefulWidget {
  @override
  _SensorsMapState createState() => _SensorsMapState();
}
