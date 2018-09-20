jQuery(function ($) {
    var data = {
        sensors: [],
        currentSensor: {
            readings: []
        },
        table: {
            collapsed: true
        },
        currentParameter: "cO2",
        mapItems: []
    }
    var map = null;
    var app = new Vue({
        el: '#app',
        data: data,
        methods: {
            initHub: initHub,
            initMap: initMap,
            initMapItems: initMapItems,
            updateMapItems: updateMapItems,
            initChart: initChart,
            initDataset: initDataset,
            updateDataset: updateDataset,
            buildRoute: buildRoute,
            expandTable: function () {
                this.table.collapsed = false;
            },
            collapseTable: function () {
                this.table.collapsed = true;
            },
            displayParameter: function (param) {
                if (this.currentParameter != param) {
                    this.currentParameter = param;
                    this.updateMapItems();
                    this.initDataset();
                }
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
    });

    $.ajax({
        type: "GET",
        url: "api/sensors",
        dataType: "JSON",
        success: function (responce) {
            app.sensors = app.sensors.concat(responce);
            app.initHub();
            app.initMap();
            app.initMapItems();
            app.initChart();
            app.initDataset();
        }
    });


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
            if (sensor.readings.length > 10) {
                sensor.readings.pop();
            }
            that.updateMapItems();
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

    function initMapItems() {
        var that = this;
        that.sensors.forEach(function (sensor, index, arrya) {
            var reading = sensor.readings[0];
            if (typeof (reading) === "undefined") {
                return;
            };
            var mapItem = {};
            var position = new google.maps.LatLng(sensor.latitude, sensor.longitude);
            mapItem.marker = new RichMarker({
                position: position,
                map: map,
                content: generateMarker(reading[that.currentParameter]),
                shadow: 'none',
            });
            mapItem.marker.setOptions({
                'opacity': 0.8
            });
            mapItem.marker.addListener('click', function () {
                that.currentSensor = sensor;
                $('#sensor-details').modal('show')
            });
            mapItem.area = new google.maps.Circle({
                strokeColor: '#FF0000',
                strokeOpacity: 0.8,
                strokeWeight: 2,
                fillColor: '#FF0000',
                fillOpacity: 0.35,
                map: map,
                center: position,
                radius: 200
            });
            that.mapItems.push(mapItem);
        })
    }
    function updateMapItems() {
        var that = this;
        this.mapItems.forEach(function (mapItem, index, array) {
            mapItem.marker.setContent(null);
        });
        this.sensors.forEach(function (sensor, index, arrya) {
            var reading = sensor.readings[0];
            if (typeof (reading) === "undefined") {
                return;
            };
            that.mapItems[index].marker.setContent(generateMarker(reading[that.currentParameter]));
        });
    }

    function generateMarker(content) {
        return '<div class="richmarker-wrapper"><p>' + content + '</p></div>';
    }

    //CHARTS
    var config = {
        type: 'line',
        data: {
            labels: [],
            datasets: []
        },
        options: {
            responsive: true,
            title: {
                display: true,
                text: 'Динамика изменения'
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
    var chart = null;
    var dataset = null;
    function initChart() {
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
            label: this.currentParameter.toUpperCase(),
            backgroundColor: "#fff",
            borderColor: "#c2c2c2c2",
            data: this.currentSensor.readings.map(function (reading) {
                return reading[that.currentParameter];
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
        dataset.data.push(reading[this.currentParameter]);
        dataset.data.shift();
        chart.update();
    }


    function buildRoute() {
        var that = this;
        var directionsService = new google.maps.DirectionsService;
        var directionsDisplay = new google.maps.DirectionsRenderer;
        directionsDisplay.setMap(map);
        directionsService.route({
            origin: new google.maps.LatLng(this.sensors[0].latitude + 400/100000, this.sensors[0].longitude),
            destination: new google.maps.LatLng(this.sensors[1].latitude, this.sensors[1].longitude - 600/100000),
            travelMode: 'DRIVING'
        }, function (response, status) {
            if (status === 'OK') {
                response.routes.forEach(function (route, index, routes) {
                    checkIfRouteIsNormal(route, index, routes, that.sensors, that.mapItems)
                })
                directionsDisplay.setDirections(response);
            } else {
                window.alert('Directions request failed due to ' + status);
            }
        });
    }


    function checkIfRouteIsNormal(route, index, routes, sensors, mapItems) {
        var Polyline = new google.maps.Polyline({
            path: route.overview_path,
            strokeColor: '#FF0000',
            strokeOpacity: 0.8,
            strokeWeight: 2,
            fillColor: '#FF0000',
            fillOpacity: 0.35,
            map:map
        });
        mapItems.forEach(function (mapItem) {
            var bounds = mapItem.area.getBounds();
            if (google.maps.geometry.poly.isLocationOnEdge(mapItem.area.getCenter(), Polyline, mapItem.area.getRadius() * (Math.pow(10,-5)))) {
                alert("Вы в опасной зоне!");
            }
        })
    }
});