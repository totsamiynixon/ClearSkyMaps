﻿using Microsoft.AspNet.SignalR;
using Services.DTO.Reading;
using Services.DTO.Sensor;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Hub;
using Web.Models.Hub;

namespace Web.Emulator
{
    public static class Emulator
    {
        private static List<SensorInfoDTO> _fakeSensors;
        public static bool IsEmulationEnabled = false;
        private static Random _emulatorRandom = new Random();

        public static void RunEmulation()
        {
            if (!IsEmulationEnabled)
            {
                IsEmulationEnabled = true;
                SeedSensors();
                StartTask();
            }
        }

        public static void StopEmulation()
        {
            if (IsEmulationEnabled)
            {
                IsEmulationEnabled = false;
                ClearSensors();
            }
        }

        public static List<SensorInfoDTO> GetSensorsData()
        {
            return _fakeSensors;
        }

        private static void StartTask()
        {
            var task = Task.Run(async () =>
            {
                while (IsEmulationEnabled)
                {
                    await Task.Delay(10000);
                    DispatchFakeData();
                }
            });
        }

        private static void SeedSensors()
        {
            _fakeSensors = new List<SensorInfoDTO>();
            var iterations = _emulatorRandom.Next(0, 20);
            for (int i = 0; i < iterations; i++)
            {
                _fakeSensors.Add(GetFakeSensor());
            }
        }


        private static void ClearSensors()
        {
            _fakeSensors = new List<SensorInfoDTO>();
        }

        private static void DispatchFakeData()
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext<ReadingsHub, IReadingsClient>();
            foreach (var fakeSensor in _fakeSensors)
            {
                hub.Clients.All.DispatchReading(new SensorReadingDispatchModel
                {
                    SensorId = fakeSensor.Id,
                    Reading = GetFakeReading()
                });
            }
        }

        private static void SeedFakeSensorWithReadings(SensorInfoDTO fakeSensor)
        {
            fakeSensor.Readings = new List<SensorReadingDTO>();
            for (int i = 0; i < 10; i++)
            {
                fakeSensor.Readings.Add(GetFakeReading());
            }
        }

        private static SensorReadingDTO GetFakeReading()
        {
            return new SensorReadingDTO
            {
                CO2 = (float)Math.Round((float)_emulatorRandom.NextDouble() * 350, 3),
                LPG = (float)Math.Round((float)_emulatorRandom.NextDouble() * 350, 3),
                CO = (float)Math.Round((float)_emulatorRandom.NextDouble() * 4, 3),
                CH4 = (float)Math.Round((float)_emulatorRandom.NextDouble() * 0.716, 3),
                Dust = (float)Math.Round((float)_emulatorRandom.NextDouble() * 350, 3),
                Temp = (float)Math.Round((float)_emulatorRandom.NextDouble() * 40, 3),
                Preassure = (float)Math.Round((float)_emulatorRandom.NextDouble() * 20, 3),
                Hum = (float)Math.Round((float)_emulatorRandom.NextDouble() * 40, 3),
                Created = DateTime.UtcNow - new TimeSpan(0, 0, 10)
            };
        }


        private static SensorInfoDTO GetFakeSensor()
        {
            var fakeLatLong = GetLocation(27.560597, 53.904588, 8000);
            var sensor = new SensorInfoDTO
            {
                Id = _fakeSensors.Count + 1,
                Latitude = fakeLatLong.randomLatitude,
                Longitude = fakeLatLong.randomLongitude
            };
            SeedFakeSensorWithReadings(sensor);
            return sensor;
        }

        private static (double randomLongitude, double randomLatitude) GetLocation(double longitude, double latittude, int radius)
        {
            Random random = _emulatorRandom;

            // Convert radius from meters to degrees
            double radiusInDegrees = radius / 111000f;

            double u = random.NextDouble();
            double v = random.NextDouble();
            double w = radiusInDegrees * Math.Sqrt(u);
            double t = 2 * Math.PI * v;
            double x = w * Math.Cos(t);
            double y = w * Math.Sin(t);

            // Adjust the x-coordinate for the shrinking of the east-west distances
            double new_x = x / Math.Cos((Math.PI / 180) * latittude);

            double foundLongitude = new_x + longitude;
            double foundLatitude = y + latittude;
            return (foundLongitude, foundLatitude);
        }
    }
}