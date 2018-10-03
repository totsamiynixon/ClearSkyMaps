<template>
  <f7-page>
    <home-sidebar></home-sidebar>
    <home-navbar></home-navbar>
    <div id="map"></div>
    <home-details-popup></home-details-popup>
  </f7-page>
</template>

<script>
import HomeSidebar from "./sidebar";
import HomeNavbar from "./navbar";
import HomeDetailsPopup from "./details.modal";

import hub from "app-hub-agent";
import map from "app-map-agent";
import api from "app-api-agent";

export default {
  data() {
    return {
      currentSensor: {
        readings: []
      },
      currentParameter: "cO2",
      mapItems: []
    };
  },
  methods: {
    openPopup() {
      console.log("clicked");
      this.$f7.popup.open(document.getElementById("home-details-popup"));
    }
  },
  components: {
    HomeSidebar,
    HomeNavbar,
    HomeDetailsPopup
  },
  onMounted() {
    map.initMap(document.getElementById("map"), () => {
      hub.on("DispatchReadingAsync", readingModel => {
        let mapItem = this.mapItems.find(function(e, i, a) {
          if (e.sensor.id == readingModel.sensorId) {
            return e.sensor;
          }
        });
        if (mapItem == null) {
          return;
        }
        mapItem.sensor.latestPollutionLevel = readingModel.latestPollutionLevel;
        mapItem.sensor.readings.unshift(readingModel.reading);
        if (mapItem.sensor.readings.length > 10) {
          mapItem.sensor.readings.pop();
        }
        map.updateArea(
          mapItem.areaId,
          mapItem.sensor.latestPollutionLevel,
          mapItem.sensor.readings[0][this.currentParameter]
        );
      });
      hub.start().then(() => {
        getAllSensors().then(response => {
          this.data.mapItems = response.data.map(sensor => {
            return {
              sensor,
              areaId: map.createNewArea(
                sensor.latestPollutionLevel,
                sensor.latitude,
                sensor.longitude,
                sensor.readings[0][this.data.currentParameter],
                () => {
                  this.openDetailsModal(sensor.id);
                }
              )
            };
          });
        });
      });
    });
  }
};
</script>
