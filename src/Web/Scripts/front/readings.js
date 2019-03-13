jQuery(function ($) {
    var map = null;
    window.CSM.readingsPage =  {
        template: '#readingsPageTemplate',
        data: function () {
            return {
                sensors: [],
                currentSensor: {
                    readings: []
                },
                table: {
                    collapsed: true
                },
                chart: {
                    currentParameter: "cO2",
                },
                markers: []
            }
        },
        mounted: function () {
            var app = this;
            $.ajax({
                type: "GET",
                url: "api/sensors",
                dataType: "JSON",
                success: function (responce) {
                    app.sensors = app.sensors.concat(responce);
                    app.initHub();
                    app.initMap();
                    app.initMarkers();
                    app.initChart();
                    app.initDataset();
                }
            });
        },
        methods: {
            initHub: initHub,
            initMap: initMap,
            initMarkers: initMarkers,
            updateMarker: updateMarker,
            initChart: initChart,
            initDataset: initDataset,
            updateDataset: updateDataset,
            expandTable: function () {
                this.table.collapsed = false;
            },
            collapseTable: function () {
                this.table.collapsed = true;
            },
            setChartCurrentParameter: function (param) {
                this.chart.currentParameter = param;
                this.initDataset();
            },
            subscribeOnSensor: function () {
                var that = this;
                window.CSM.askForPermissioToReceiveNotifications().then(function (token) {
                    var sensors = JSON.parse(localStorage.getItem("subscribedOnSensors")) || [];
                    if (sensors.indexOf(that.currentSensor.id) == -1) {
                        $.ajax({
                            type: "POST",
                            url: "api/notifications",
                            contentType: "application/json",
                            data: JSON.stringify({
                                sensorId: that.currentSensor.id,
                                registrationToken: token
                            }),
                            success: function (responce) {
                                sensors.push(that.currentSensor.id);
                                localStorage.setItem("subscribedOnSensors", JSON.stringify(sensors));
                                alert("Успешно подписан");
                            },
                            error: function (error) {
                                alert("Ошибка");
                            }
                        });
                    }
                    else {
                        alert("Уже подписан!");
                    }
                });
            },
            unsubscribeFromSensor: function () {
                var that = this;
                window.CSM.askForPermissioToReceiveNotifications().then(function (token) {
                    var sensors = JSON.parse(localStorage.getItem("subscribedOnSensors")) || [];
                    if (sensors.indexOf(that.currentSensor.id) == -1) {
                        alert("Уже отписан");
                    }
                    $.ajax({
                        type: "DELETE",
                        url: "api/notifications",
                        contentType: "application/json",
                        data: JSON.stringify({
                            sensorId: that.currentSensor.id,
                            registrationToken: token
                        }),
                        success: function (responce) {
                            var index = sensors.indexOf(that.currentSensor.id);
                            if (index != -1) {
                                sensors.splice(index, 1);
                            }
                            localStorage.setItem("subscribedOnSensors", JSON.stringify(sensors));
                            alert("Успешно отписан");
                        },
                        error: function (error) {
                            alert("Ошибка");
                        }
                    });
                })
            }
        },
        computed: {
            isSubscribed: function () {
                var that = this;
                var sensors = JSON.parse(localStorage.getItem("subscribedOnSensors")) || [];
                return sensors.indexOf(that.currentSensor.id) != -1;
            }
        },
        watch: {
            currentSensor:
            {
                handler: function (val) {
                    this.updateDataset();
                },
                deep: true
            }
        }
    };



    //HUB
    function initHub() {
        var that = this;
        var chat = $.connection.readingsHub;
        chat.client.dispatchReading = function (readingModel) {
            var sensor = that.sensors.find(function (e, i, a) {
                if (e.id == readingModel.sensorId) {
                    return e;
                }
            });
            if (sensor == null) {
                return;
            }
            sensor.readings.unshift(readingModel.reading);
            sensor.pollutionLevel = readingModel.pollutionLevel;
            if (sensor.readings.length == 11) {
                sensor.readings.pop();
            }
            that.updateMarker(sensor);
        };
        $.connection.hub.start().done(function () {

        });
    }

    //MAP
    function initMap() {
        // Styles a map in night mode.
        map = new google.maps.Map(document.getElementById('map'),
            {
                center: {
                    lat: 53.904502,
                    lng: 27.561261
                }
                ,
                zoom: 12,
                scrollwheel: false,
                mapTypeControl: false,
                styles: [
                    {
                        "featureType": "administrative",
                        "elementType": "all",
                        "stylers": [
                            {
                                "saturation": "-100"
                            }
                        ]
                    }
                    ,
                    {
                        "featureType": "administrative.province",
                        "elementType": "all",
                        "stylers": [
                            {
                                "visibility": "off"
                            }
                        ]
                    }
                    ,
                    {
                        "featureType": "landscape",
                        "elementType": "all",
                        "stylers": [
                            {
                                "saturation": -100
                            }
                            ,
                            {
                                "lightness": 65
                            }
                            ,
                            {
                                "visibility": "on"
                            }
                        ]
                    }
                    ,
                    {
                        "featureType": "poi",
                        "elementType": "all",
                        "stylers": [
                            {
                                "saturation": -100
                            }
                            ,
                            {
                                "lightness": "50"
                            }
                            ,
                            {
                                "visibility": "simplified"
                            }
                        ]
                    }
                    ,
                    {
                        "featureType": "road",
                        "elementType": "all",
                        "stylers": [
                            {
                                "saturation": "-100"
                            }
                        ]
                    }
                    ,
                    {
                        "featureType": "road.highway",
                        "elementType": "all",
                        "stylers": [
                            {
                                "visibility": "simplified"
                            }
                        ]
                    }
                    ,
                    {
                        "featureType": "road.arterial",
                        "elementType": "all",
                        "stylers": [
                            {
                                "lightness": "30"
                            }
                        ]
                    }
                    ,
                    {
                        "featureType": "road.local",
                        "elementType": "all",
                        "stylers": [
                            {
                                "lightness": "40"
                            }
                        ]
                    }
                    ,
                    {
                        "featureType": "transit",
                        "elementType": "all",
                        "stylers": [
                            {
                                "saturation": -100
                            }
                            ,
                            {
                                "visibility": "simplified"
                            }
                        ]
                    }
                    ,
                    {
                        "featureType": "water",
                        "elementType": "geometry",
                        "stylers": [
                            {
                                "hue": "#ffff00"
                            }
                            ,
                            {
                                "lightness": -25
                            }
                            ,
                            {
                                "saturation": -97
                            }
                        ]
                    }
                    ,
                    {
                        "featureType": "water",
                        "elementType": "labels",
                        "stylers": [
                            {
                                "lightness": -25
                            }
                            ,
                            {
                                "saturation": -100
                            }
                        ]
                    }
                ]
            }
        );
    }

    function initMarkers() {
        var that = this;
        that.sensors.forEach(function (sensor, index, arrya) {
            var reading = sensor.readings[0];
            if (typeof (reading) === "undefined") {
                return;
            }
            var marker = {
                sensorId: sensor.id,
                value: createMarker(sensor)
            };
            marker.value.addListener('click', function () {
                that.currentSensor = sensor;
                $('#sensor-details').modal('show')
            });
            that.markers.push(marker);
        })
    }

    function createMarker(sensor) {
        var that = this;
        var marker = new RichMarker({
            position: new google.maps.LatLng(sensor.latitude, sensor.longitude),
            map: map,
            content: generateContent(sensor.pollutionLevel),
            shadow: 'none',
        });
        marker.setOptions({
            'opacity': 0.8
        });
        return marker;
    }
    function updateMarker(sensor) {
        var that = this;
        var marker = that.markers.find(function (marker) {
            return marker.sensorId == sensor.id;
        });
        if (marker == null) {
            return;
        }
        marker.value.setMap(null);
        marker.value = createMarker(sensor);
        marker.value.addListener('click', function () {
            that.currentSensor = sensor;
            $('#sensor-details').modal('show')
        });
    }

    function generateContent(content) {
        return '<div class="richmarker-wrapper"><p>' + content + '</p></div>';
    }

    //CHARTS
   
    var chart = null;
    var dataset = null;
    function initChart() {
        var config = {
            type: 'line',
            data: {
                labels: [],
                datasets: []
            },
            options: {
                responsive: true,
                title: {
                    display: false
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
        var ctx = document.getElementById('chart').getContext('2d');
        chart = new Chart(ctx, config);
    }

    function initDataset() {
        if (chart == null) {
            return;
        }
        chart.data.datasets = [];
        var that = this;
        chart.data.labels = this.currentSensor.readings.map(function (reading) {
            return moment(reading.created).format('h:mm:ss');
        });
        dataset = {
            label: this.chart.currentParameter.toUpperCase(),
            backgroundColor: "#fff",
            borderColor: "#c2c2c2c2",
            data: this.currentSensor.readings.map(function (reading) {
                return reading[that.chart.currentParameter];
            }),
            fill: false,
        }
        chart.data.datasets.push(dataset);
        chart.update();
    }

    function updateDataset() {
        var reading = this.currentSensor.readings[0];
        if (typeof (reading) === "undefined") {
            return;
        }
        chart.data.labels.push(moment(reading.created).format('h:mm:ss'));
        chart.data.labels.shift();
        dataset.data.push(reading[this.chart.currentParameter]);
        dataset.data.shift();
        chart.update();
    }
});