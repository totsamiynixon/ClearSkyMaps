jQuery(function ($) {
    window.CSM_Admin.addModule("PortableSensorDetails", function (options) {
        var app = new Vue({
            el: '#app',
            template: "#portableSensorDetailsPageTemplate",
            data: {
                hub: {
                    instance: null,
                    isActive: false
                },
                map: null,
                sensor: {
                    id: options.id,
                    latestReadings: {},
                    latitude: null,
                    longitude: null
                },
                sensorMarker: null
            },
            methods: {
                initHub: initHub,
                initMarker: initMarker,
                updateMarker: updateMarker,
                initMap: initMap
            },
            mounted: function () {
                var that = this;
                that.initHub();
                ymaps.ready(function () {
                    that.initMap();
                });
            }
        })


        //HUB
        function initHub() {
            var that = this;
            that.hub.instance = $.connection.adminPortableSensorHub;
            that.hub.instance.client.dispatchReading = function (readingModel) {
                that.sensor.latestReadings = readingModel;
            };
            that.hub.instance.client.dispatchCoordinates = function (coordinatesModel) {
                that.sensor.latitude = coordinatesModel.latitude;
                that.sensor.longitude = coordinatesModel.longitude;
                if (!that.sensorMarker) {
                    that.initMarker();
                }
                else {
                    that.updateMarker();
                }
            };
            $.connection.hub.start().done(function () {
                that.hub.instance.server.listenForSensor(that.sensor.id);
                that.hub.isActive = true;
                $.connection.hub.disconnected(function () {
                    if (!that.hub.isActive) {
                        return;
                    }
                    setTimeout(function () {
                        $.connection.hub.start();
                    }, 5000);
                });
            });
        }

        function initMap() {
            var that = this;
            that.map = new ymaps.Map("map", {
                center: [53.904502, 27.561261],
                zoom: 18,
                controls: ["zoomControl"]
            },
                {
                    searchControlProvider: 'yandex#search'
                });
        }
        function initMarker() {
            var that = this;
            that.sensorMarker = new ymaps.Placemark([that.sensor.latitude, that.sensor.longitude], {
                balloonContent: 'Положение датчика'
            }, {
                    preset: 'islands#circleIcon',
                    iconColor: '#3caa3c'
                });
            that.map.setCenter([that.sensor.latitude, that.sensor.longitude], 18);
            that.map.geoObjects.add(that.sensorMarker);
        }
        function updateMarker() {
            var that = this;
            that.sensorMarker.geometry.setCoordinates([that.sensor.latitude, that.sensor.longitude]);
        }
    })
});