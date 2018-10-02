jQuery(function ($) {
    var data = {
        currentSensor: {
            readings: []
        },
        currentPossiblePollutionLevel: 0,
        table: {
            collapsed: true
        },
        currentParameter: "cO2",
        mapItems: [],
        wayPoint: {
            buffer: null,
            from: null,
            to: null
        },
        googleServices: {
            directions: null,
            display: null,
            currentRoute: null
        }
    }
    var map = null;
    var app = new Vue({
        el: '#app',
        data: data,
        methods: {
            initHub: initHub,
            initMap: initMap,
            initWayPoint: initWayPoint,
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
            },
            setWayPoint: function (direction) {
                this.wayPoint[direction].value = this.wayPoint.buffer;
                this.wayPoint[direction].marker.setOptions({ position: this.wayPoint.buffer })
                this.wayPoint[direction].marker.setMap(map);
                this.wayPoint.buffer = null;
                if (this.wayPoint.from.value != null && this.wayPoint.to.value != null) {
                    this.buildRoute();
                }
            },
            hideWayPoints: function () {
                this.wayPoint.from.marker.setMap(null);
                this.wayPoint.to.marker.setMap(null);
            },
            handleContextMenu: handleContextMenu
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
        success: function (sensors) {
            sensors.forEach(function (sensor) {
                app.mapItems.push({
                    sensor: sensor
                });
            });
            app.initHub();
            app.initMap();
            app.initMapItems();
            app.initWayPoint();
            app.initChart();
            app.initDataset();
        }
    });


    //HUB
    function initHub() {
        var that = this;
        var chat = $.connection.readingsHub;
        chat.client.dispatchReading = function (readingModel) {
            var mapItem = that.mapItems.find(function (e, i, a) {
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
            that.updateMapItems();
        };
        $.connection.hub.start().done(function () {

        });
    }
    //MAP
    function initMap() {
        var that = this;
        $("#map-context-menu").hide();
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
        map.addListener("rightclick", that.handleContextMenu("map"));
        $(window).click(function () {
            $("#map-context-menu").hide();
        })
    }

    function initWayPoint() {
        this.wayPoint.from = {
            value: null,
            marker: new google.maps.Marker({
                position: { lat: 0, lng: 0 },
                title: 'Откуда'
            })
        };
        this.wayPoint.to = {
            value: null,
            marker: new google.maps.Marker({
                position: { lat: 0, lng: 0 },
                title: 'Куда'
            })
        }

    }

    function initMapItems() {
        var that = this;
        that.mapItems.forEach(function (mapItem, index, arrya) {
            var reading = mapItem.sensor.readings[0];
            if (typeof (reading) === "undefined") {
                return;
            };
            var position = new google.maps.LatLng(mapItem.sensor.latitude, mapItem.sensor.longitude);
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
                that.currentSensor = mapItem.sensor;
                $('#sensor-details').modal('show')
            });
            mapItem.marker.addListener("rightclick", that.handleContextMenu("map"));
            mapItem.area = new google.maps.Circle({
                strokeColor: getStrokeColorByPollutionLevel(that.currentPossiblePollutionLevel),
                strokeOpacity: 0.8,
                strokeWeight: 2,
                fillColor: getFillColorByPollutionLevel(that.currentPossiblePollutionLevel),
                fillOpacity: 0.35,
                map: map,
                center: position,
                radius: 1000
            });
            mapItem.area.addListener("rightclick", that.handleContextMenu("map"));
        })
    }
    function updateMapItems() {
        var that = this;
        this.mapItems.forEach(function (mapItem, index, array) {
            mapItem.marker.setContent(null);
        });
        this.mapItems.forEach(function (mapItem, index, arrya) {
            var reading = mapItem.sensor.readings[0];
            if (typeof (reading) === "undefined") {
                return;
            };
            mapItem.marker.setContent(generateMarker(reading[that.currentParameter]));
            mapItem.area.setOptions({
                strokeColor: getStrokeColorByPollutionLevel(mapItem.sensor.latestPollutionLevel),
                fillColor: getFillColorByPollutionLevel(mapItem.sensor.latestPollutionLevel)
            });
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
        if (that.googleServices.directions == null) {
            that.googleServices.directions = new google.maps.DirectionsService;
        }
        if (that.googleServices.display != null) {
            that.googleServices.display.setMap(null);
            that.googleServices.display = null;
        }
        that.googleServices.display = new google.maps.DirectionsRenderer;
        that.googleServices.display.setMap(map);
        that.googleServices.directions.route({
            origin: that.wayPoint.from.value,
            destination: that.wayPoint.to.value,
            travelMode: 'WALKING',
            provideRouteAlternatives: true
        }, function (response, status) {
            var unupproptiateMapItems = that.mapItems.filter(function (mapItem) {
                return mapItem.sensor.latestPollutionLevel > that.currentPossiblePollutionLevel
            });
            var appropriateRoute = null;
            if (status === 'OK') {
                response.routes.forEach(function (route, index, routes) {
                    if (appropriateRoute == null && checkIfRouteIsNormal(route, index, routes, unupproptiateMapItems)) {
                        appropriateRoute = route;
                    }
                });
                if (appropriateRoute != null) {
                    response.routes = [appropriateRoute];
                    that.googleServices.display.setDirections(response);
                    that.googleServices.currentRoute = appropriateRoute;
                    that.hideWayPoints();
                }
                else {
                    window.alert('Не было найдено подходящего маршрута!');
                }
            } else {
                window.alert('Не было найдено подходящего маршрута из-за ' + status);
            }
        })
    }

    function checkIfRouteIsNormal(route, index, routes, mapItems) {
        var Polyline = new google.maps.Polyline({
            path: route.overview_path,
            strokeColor: '#FF0000',
            strokeOpacity: 0.8,
            strokeWeight: 2,
            fillColor: '#FF0000',
            fillOpacity: 0.35,
        });
        var isAppropriate = true;
        mapItems.forEach(function (mapItem) {
            if (isAppropriate) {
                isAppropriate = !google.maps.geometry.poly.isLocationOnEdge(mapItem.area.getCenter(), Polyline, mapItem.area.getRadius() * (Math.pow(10, -5)));
            }
        });
        return isAppropriate;
    }

    function getStrokeColorByPollutionLevel(pollutionLevel) {
        switch (pollutionLevel) {
            case 0:
                return "#3cf24b";
            case 1:
                return "#e2ac2d";
            case 2:
                return "#f90000"
        }
    }

    function getFillColorByPollutionLevel(pollutionLevel) {
        switch (pollutionLevel) {
            case 0:
                return "#08e01a";
            case 1:
                return "#d3c60e";
            case 2:
                return "#a01818"
        }
    }

    function handleContextMenu(target) {
        var that = this;
        switch (target) {
            case "map": {
                var mapFcn = function (event) {
                    that.wayPoint.buffer = event.latLng;
                    $("#map-context-menu").css({ position: "fixed", top: event.wa.pageY, left: event.wa.pageX });
                    $("#map-context-menu").show();
                };
                return mapFcn;
            }
        }
    }
});