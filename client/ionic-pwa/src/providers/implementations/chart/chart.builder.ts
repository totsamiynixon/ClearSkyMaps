import { Injectable } from "@angular/core";
import moment from "moment";
import Chart from "chart.js";
import { Sensor } from "../../../models/sensor.model";
import { Parameters } from "../../../models/parameters.enum";
import { ChartBuilder } from "../../inerfaces/chart/chart.builder";

@Injectable()
export class ChartJSChartBuilder implements ChartBuilder {
  chart: Chart;
  initChart(element: HTMLCanvasElement): void {
    var ctx = element.getContext("2d");
    this.chart = new Chart(ctx, {
      type: "line",
      data: {
        labels: [],
        datasets: []
      },
      options: {
        responsive: true,
        title: {
          display: true,
          text: "Динамика изменения"
        },
        tooltips: {
          mode: "index",
          intersect: false
        },
        hover: {
          mode: "nearest",
          intersect: true
        },
        scales: {
          xAxes: [
            {
              display: true,
              scaleLabel: {
                display: true,
                labelString: "Cнято"
              }
            }
          ],
          yAxes: [
            {
              display: true,
              scaleLabel: {
                display: true,
                labelString: "Значение"
              }
            }
          ]
        }
      }
    });
  }
  updateDataset(currentSensor: Sensor, currentParameter: Parameters): void {
    this.chart.data.datasets = [];
    this.chart.data.labels = currentSensor.readings.map(function(reading) {
      return moment(reading.created).format("h:mm:ss");
    });
    var reading = currentSensor.readings[0];
    if (typeof reading === "undefined") {
      return;
    }
    let dataset = {
      label: Parameters[currentParameter].toUpperCase(),
      backgroundColor: "#fff",
      borderColor: "#c2c2c2c2",
      data: currentSensor.readings.map(function(reading) {
        return reading[Parameters[currentParameter]];
      }),
      fill: false
    };
    this.chart.data.datasets.push(dataset);
    this.chart.update();
  }
}
