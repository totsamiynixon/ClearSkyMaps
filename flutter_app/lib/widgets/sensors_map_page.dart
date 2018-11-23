import 'package:flutter/material.dart';
import 'package:flutter_map/flutter_map.dart';
import 'package:latlong/latlong.dart';

class MapsDemo extends StatelessWidget {
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
        center: new LatLng(51.5, -0.09),
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
          markers: [
            new Marker(
              width: 80.0,
              height: 80.0,
              point: new LatLng(51.5, -0.09),
              builder: (ctx) => new Container(
                    child: InkWell(
                      child: FlutterLogo(),
                      onTap: () {
                        Navigator.pushNamed(ctx, '/details');
                      },
                    ),
                  ),
            ),
          ],
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
          onPressed: () {
            Navigator.pushNamed(context, "/details");
          },
        )
      ],
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
