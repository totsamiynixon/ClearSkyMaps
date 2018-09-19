jQuery(function ($) {
    var data = {
        currentParameter: null,
        currentProperty: null,
        startPeriod: null,
        endPeriod: null,
        everyNth: null,
        parameters: [],
        readings: {
            data: [],
            count: 15
        },
        result: null,
    }
    var app = new Vue({
        el: '#app',
        data: data,
        methods: {
            getParameters: getParameters,
            setCurrentParameter: setCurrentParameter,
            setCurrentProperty: setCurrentProperty,
            setStartPeriod: setStartPeriod,
            setEndPeriod: setEndPeriod,
            exportData: exportData,
            getData: getData,
            getResult: getResult,
            getIZA: getIZA
        },
        computed: {
            dataCanBeLoaded: dataCanBeLoaded,
            resultCanBeLoaded: resultCanBeLoaded
        },
        created: function () {
            this.getParameters();
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
            currentParameter: function () {
                this.getResult();
            },
            currentProperty: function () {
                this.getResult();
            },
            startPeriod: proceedWatch,
            endPeriod: proceedWatch,
            everyNth: proceedWatch
        }
    })

    function proceedWatch() {
        this.getResult();
        this.readings.data = [];
        this.getData();
    }
    function exportData() {
        var url = "home/ExportValuesByPeriod?startPeriod=" + this.startPeriod + "&endPeriod=" + this.endPeriod + "&sensorId=" + window.settings.sensorId + "&everyNth=" + this.everyNth;
        var win = window.open(url, '_blank');
    }

    function setCurrentParameter(parameter) {
        this.currentParameter = parameter;
    }

    function setCurrentProperty(property) {
        this.currentProperty = property;
    }

    function setStartPeriod(startPeriod) {
        this.startPeriod = moment.utc(startPeriod).format("MM/DD/YYYY")
    }

    function setEndPeriod(endPeriod) {
        this.endPeriod = moment.utc(endPeriod).format("MM/DD/YYYY")
    }


    function dataCanBeLoaded() {
        return this.startPeriod != null &&
            this.endPeriod != null
    }

    function resultCanBeLoaded() {
        return this.dataCanBeLoaded && this.currentParameter != null && this.currentProperty != null;
    }
    function getData() {
        if (!this.dataCanBeLoaded) {
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
                count: this.readings.count,
                everyNth: this.everyNth
            },
            dataType: "JSON",
            success: function (response) {
                app.readings.data = app.readings.data.concat(response);
            }
        });
    }

    function getParameters() {
        $.ajax({
            type: "GET",
            url: "api/analytics/getParameters",
            dataType: "JSON",
            success: function (response) {
                app.parameters = response;
            }
        });
    }
    function getResult() {
        if (!this.resultCanBeLoaded) {
            return;
        }
        $.ajax({
            type: "GET",
            url: "api/analytics/get" + this.currentProperty,
            data: {
                parameterName: this.currentParameter,
                startPeriod: this.startPeriod,
                endPeriod: this.endPeriod,
                sensorId: window.settings.sensorId,
                everyNth: this.everyNth
            },
            dataType: "JSON",
            success: function (response) {
                app.result = response;
            }
        });
    }
    function getIZA() {
        $.ajax({
            type: "GET",
            url: "api/analytics/getIZA",
            data: {
                sensorId: window.settings.sensorId
            },
            dataType: "JSON",
            success: function (response) {
                app.result = response;
            }
        });
    }
});