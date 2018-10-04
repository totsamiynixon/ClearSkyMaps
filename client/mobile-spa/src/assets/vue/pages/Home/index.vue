<template>
  <f7-page>
    <home-sidebar @parameterSelected="setCurrentParameter"
                  :currentParameter="currentParameter"></home-sidebar>
    <home-navbar></home-navbar>
    <div id="map"></div>
    <home-details-popup :sensor="currentSensor"
                        :currentParameter="currentParameter"></home-details-popup>
  </f7-page>
</template>

<script>
import HomeSidebar from "./sidebar";
import HomeNavbar from "./navbar";
import HomeDetailsPopup from "./details.modal";

import chart from "../../../js/services/chart.js";
import hub from "../../../js/services/hub.js";
import map from "../../../js/services/map.js";
import api from "../../../js/services/api.js";

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
    openDetailsModal(sensorId) {
      this.currentSensor = this.mapItems.find(
        f => f.sensor.id == sensorId
      ).sensor;
      this.$f7.popup.open(document.getElementById("home-details-popup"));
      chart.updateDataset(this.currentSensor, this.currentParameter);
    },
    setCurrentParameter(currentParameter) {
      this.currentParameter = currentParameter;
      this.mapItems.forEach(mapItem => {
        map.updateArea(
          mapItem.areaId,
          mapItem.sensor.latestPollutionLevel,
          mapItem.sensor.readings[0][this.currentParameter]
        );
      });
    }
  },
  components: {
    HomeSidebar,
    HomeNavbar,
    HomeDetailsPopup
  },
  mounted() {
    chart.initChart(document.getElementById("chart"));
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
        api.getAllSensors().then(response => {
          this.mapItems = response.data.map(sensor => {
            return {
              sensor,
              areaId: map.createNewArea(
                sensor.latestPollutionLevel,
                sensor.latitude,
                sensor.longitude,
                sensor.readings[0][this.currentParameter],
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
<style>
#map {
  position: static !important;
}
</style>
