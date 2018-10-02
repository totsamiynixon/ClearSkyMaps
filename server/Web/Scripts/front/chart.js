jQuery(function ($) {
    var chart = null;
    var dataset = null;
    var data = {
        startPeriod: null,
        endPeriod: null,
        readings: {
            data: [],
            count: 15
        },
        currentParameter: "cO2",
    }
    var app = new Vue({
        el: '#app',
        data: data,
        methods: {
            setStartPeriod: setStartPeriod,
            setEndPeriod: setEndPeriod,
            exportData: exportData,
            getData: getData,
            initDataset: initDataset,
            initChart: initChart,
            displayParameter: function (param) {
                if (this.currentParameter != param) {
                    this.currentParameter = param;
                    this.initDataset();
                }
            }
        },
        computed: {
            valid: valid
        },
        mounted: function () {
            var vm = this
            $('#startPeriod').datepicker({
                time: false, onSelect: function (date) {
                    vm.setStartPeriod(date);
                }
            });
            $('#endPeriod').datepicker({
                time: false, onSelect: function (date) {
                    vm.setEndPeriod(date);
                }
            });
        },
        watch: {
            startPeriod: function () {
                this.readings.data = [];
                this.getData();
            },
            endPeriod: function () {
                this.readings.data = [];
                this.getData();
            }
        }
    })
    setTimeout(function () {
        app.initChart();
    }, 1000);
    function exportData() {
        var url = "home/ExportValuesByPeriod?startPeriod=" + this.startPeriod + "&endPeriod=" + this.endPeriod + "&sensorId=" + window.settings.sensorId;
        var win = window.open(url, '_blank');
    }

    function setStartPeriod(startPeriod) {
        this.startPeriod = moment.utc(startPeriod).format("MM/DD/YYYY")
    }

    function setEndPeriod(endPeriod) {
        this.endPeriod = moment.utc(endPeriod).format("MM/DD/YYYY")
    }

    function valid() {
        return this.startPeriod != null && this.endPeriod != null;
    }
    function getData() {
        if (!this.valid) {
            return;
        }
        $.ajax({
            type: "GET",
            url: "api/analytics/getAll",
            data: {
                startPeriod: this.startPeriod,
                endPeriod: this.endPeriod,
                sensorId: window.settings.sensorId,
                skip: this.readings.data.length,
                count: this.readings.count
            },
            dataType: "JSON",
            success: function (response) {
                app.readings.data = app.readings.data.concat(response);
                app.initDataset();
            }
        });
    }
    var config = {
        type: "line",
        data: {
            labels: [],
            datasets: []
        },
        options: {
            responsive: true,
            title: {
                display: true
            },
            tooltips: {
                mode: 'index',
                intersect: false,
            },
            hover: {
                mode: 'nearest',
                intersect: true
            },
            scales: {
                xAxes: [{
                    display: true,
                    scaleLabel: {
                        display: true,
                        labelString: 'Cнято'
                    }
                }],
                yAxes: [{
                    display: true,
                    scaleLabel: {
                        display: true,
                        labelString: 'Значение'
                    }
                }]
            }
        }
    };

    function initChart() {
        var ctx = document.getElementById('chart').getContext('2d');
        chart = new Chart(ctx, config);
    }

    function initDataset() {
        chart.data.datasets = [];
        chart.options.title.text = "График " + this.currentParameter.toUpperCase();
        var that = this;
        chart.data.labels = this.readings.data.map(function (reading) {
            return moment(reading.created).format('h:mm:ss');
        });
        dataset = {
            label: this.currentParameter.toUpperCase(),
            backgroundColor: "#fff",
            borderColor: "#c2c2c2c2",
            data: this.readings.data.map(function (reading) {
                return reading[that.currentParameter];
            }),
            fill: false,
        }
        chart.data.datasets.push(dataset);
        chart.update();
    }
});