<template>
  <div>
    <home-sidebar @parameterSelected="setCurrentParameter"
                  :currentParameter="currentParameter"></home-sidebar>
    <f7-page>
      <f7-page-content class="map-page">
        <home-navbar :routerSheetOpened="routerSheet.opened"
                     @routerSheetOpened="handleRouterSheetOpened"></home-navbar>
        <div id="map"></div>
        <home-details-popup :sensor="currentSensor"
                            :currentParameter="currentParameter"></home-details-popup>
        <home-footer-sheet :opened="routerSheet.opened"
                           @closed="handleRouterSheetClosed"></home-footer-sheet>
      </f7-page-content>
    </f7-page>
  </div>
</template>

<script>
import HomeSidebar from "./sidebar";
import HomeNavbar from "./navbar";
import HomeDetailsPopup from "./details.modal";
import HomeFooterSheet from "./footer.sheet";

import chart from "../../services/chart.js";
import hub from "../../services/hub.js";
import map from "../../services/map.js";
import api from "../../services/api.js";

import toTimeFilter from "../../filters/toTime.filter.js";
export default {
  data() {
    return {
      routerSheet: {
        opened: true
      },
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
      this.$f7.sheet.close(".router-sheet");
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
    },
    handleRouterSheetClosed() {
      this.routerSheet.opened = false;
    },
    handleRouterSheetOpened() {
      this.routerSheet.opened = true;
    }
  },
  components: {
    HomeSidebar,
    HomeNavbar,
    HomeDetailsPopup,
    HomeFooterSheet
  },
  filters: {
    toTime: toTimeFilter
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
<style scoped>
#map {
  position: static !important;
}
.map-page {
  overflow: hidden;
}
</style>
