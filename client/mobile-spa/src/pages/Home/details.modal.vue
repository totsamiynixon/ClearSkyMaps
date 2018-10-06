<template>
  <f7-popup id="home-details-popup">
    <f7-page :page-content="false">
      <f7-navbar>
        <f7-nav-left>
          <f7-link popup-close
                   @click="()=>{table.collapsed = true;}"
                   icon="icon-back"></f7-link>
        </f7-nav-left>
        <f7-nav-title>
          Детали
        </f7-nav-title>
        <f7-nav-right v-if="currentTab == 2">
          <f7-link @click="table.collapsed = !table.collapsed">
            <f7-icon v-show="table.collapsed"
                     fa="expand"></f7-icon>
            <f7-icon v-show="!table.collapsed"
                     fa="compress"></f7-icon>
          </f7-link>
        </f7-nav-right>
      </f7-navbar>
      <f7-toolbar tabbar
                  labels>
        <f7-link tab-link="#tab-1"
                 @click="currentTab = 1"
                 tab-link-active
                 icon-md="fa:line-chart"
                 icon-ios="fa:line-chart"
                 text="График"></f7-link>
        <f7-link tab-link="#tab-2"
                 icon-md="fa:table"
                 icon-ios="fa:table"
                 text="Таблица"
                 @click="currentTab = 2"></f7-link>
      </f7-toolbar>
      <f7-tabs>
        <f7-tab id="tab-1"
                class="page-content"
                tab-active>
          <f7-block>
            <div class="chart-container"
                 style="position: relative;">
              <canvas id="chart"></canvas>
            </div>
          </f7-block>
        </f7-tab>
        <f7-tab id="tab-2"
                class="page-content">
          <f7-block>
            <div class="data-table">
              <table>
                <thead>
                  <tr>
                    <th class="numeric-cell">Снято</th>
                    <th class="numeric-cell">CO2</th>
                    <th class="numeric-cell">LPG</th>
                    <th class="numeric-cell">CO</th>
                    <th class="numeric-cell">CH4</th>
                    <th class="numeric-cell">Пыль</th>
                    <th class="numeric-cell">Т-ра</th>
                    <th class="numeric-cell">Hum</th>
                    <th class="numeric-cell">Давление</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(item,index) in sensor.readings"
                      :key="index"
                      :class="{'d-none' : index > 0 && table.collapsed == true}">
                    <td class="numeric-cell">{{item.cO2}}</td>
                    <td class="numeric-cell">{{item.lpg}}</td>
                    <td class="numeric-cell">{{item.co}}</td>
                    <td class="numeric-cell">{{item.cH4}}</td>
                    <td class="numeric-cell">{{item.dust}}</td>
                    <td class="numeric-cell">{{item.temp}}</td>
                    <td class="numeric-cell">{{item.hum}}</td>
                    <td class="numeric-cell">{{item.preassure}}</td>
                    <td class="numeric-cell">{{item.created | toTime}}</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </f7-block>
        </f7-tab>
      </f7-tabs>
    </f7-page>
  </f7-popup>
</template>
<script>
import toTimeFilter from "../../filters/toTime.filter.js";
export default {
  data() {
    return {
      table: {
        collapsed: true
      },
      currentTab: 1
    };
  },
  filters: {
    toTime: toTimeFilter
  },
  props: ["sensor"]
};
</script>

<style scoped>
.d-none {
  display: none;
}
</style>
