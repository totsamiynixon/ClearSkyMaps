import Chart from "chart.js";
import moment from "moment";
var chart, dataset;
export default {
  initChart,
  updateDataset
};
function initChart(element) {
  var ctx = element.getContext("2d");
  chart = new Chart(ctx, {
    type: "line",
    maintainAspectRatio: false,
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
  ctx.height = 500;
}

function updateDataset(currentSensor, currentParameter) {
  chart.data.datasets = [];
  chart.data.labels = currentSensor.readings.map(function(reading) {
    return moment(reading.created).format("h:mm:ss");
  });
  var reading = currentSensor.readings[0];
  if (typeof reading === "undefined") {
    return;
  }
  dataset = {
    label: currentParameter.toUpperCase(),
    backgroundColor: "#fff",
    borderColor: "#c2c2c2c2",
    data: currentSensor.readings.map(function(reading) {
      return reading[currentParameter];
    }),
    fill: false
  };
  chart.data.datasets.push(dataset);
  chart.data.labels.push(moment(reading.created).format("h:mm:ss"));
  chart.data.labels.shift();
  dataset.data.push(reading[currentParameter]);
  dataset.data.shift();
  chart.update();
}
