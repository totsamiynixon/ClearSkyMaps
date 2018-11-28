import 'dart:convert';

class Reading {
  Reading() {}
  num cO2;

  num lPG;

  num cO;

  num cH4;

  num dust;

  num temp;

  num hum;

  num preassure;

  DateTime created;

  factory Reading.fromJson(Map<String, dynamic> data) {
    var reading = new Reading();
    reading.cO = data['co'];
    reading.cH4 = data['cH4'];
    reading.cO2 = data['cO2'];
    reading.dust = data['dust'];
    reading.hum = data['hum'];
    reading.lPG = data['lPG'];
    reading.preassure = data['preassure'];
    reading.temp = data['temp'];
    reading.created = DateTime.now();
    //reading.created = DateTime.parse(data['created'].toString());
    return reading;
  }
}
