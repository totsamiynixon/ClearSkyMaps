using ArduinoServer.Controllers.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Web.Data;
using Web.Data.Models;
using Web.Helpers;
using Web.Models.Api.Readings;
using Web.Models.Api.Sensor;
using Z.EntityFramework.Plus;

namespace Web.Emulation
{
    public static class Emulator
    {
        private static bool IsEmulationEnabled = false;
        private static Random _emulatorRandom = new Random();


        public static async Task RunEmulationAsync()
        {
            if (!IsEmulationEnabled)
            {
                IsEmulationEnabled = true;
                await SeedSensorsAsync();
                StartTask();
            }
        }

        public static void StopEmulation()
        {
            IsEmulationEnabled = false;
        }

        private static void StartTask()
        {
            var task = Task.Run(async () =>
            {
                while (IsEmulationEnabled)
                {
                    await DispatchFakeDataAsync();
                    await Task.Delay(10000);
                }
            });
        }

        private static async Task SeedSensorsAsync()
        {
            var currentSensors = await DatabaseHelper.GetSensorsAsync();
            if (currentSensors.Any())
            {
                return;
            }
            var iterations = _emulatorRandom.Next(0, 20);
            for (int i = 0; i < iterations; i++)
            {
                var fakeSensor = GetFakeSensor();
                await DatabaseHelper.AddSensorAsync(fakeSensor.Latitude, fakeSensor.Longitude);
                foreach (var reading in fakeSensor.Readings)
                {
                    await DatabaseHelper.AddReadingAsync(reading);
                }
            }

        }

        private static async Task DispatchFakeDataAsync()
        {
            var sensors = await DatabaseHelper.GetSensorsAsync();
            Parallel.ForEach(sensors, async (sensor) =>
            {
                var controller = new ReadingsController();
                var fakeReading = GetFakeReading(sensor.TrackingKey);
                await controller.PostReading(fakeReading);
            });
        }

        private static SaveReadingModel GetFakeReading(string trackingKey)
        {
            return new SaveReadingModel
            {
                CO2 = (float)Math.Round((float)_emulatorRandom.NextDouble() * 350, 3),
                LPG = (float)Math.Round((float)_emulatorRandom.NextDouble() * 350, 3),
                CO = (float)Math.Round((float)_emulatorRandom.NextDouble() * 4, 3),
                CH4 = (float)Math.Round((float)_emulatorRandom.NextDouble() * 0.716, 3),
                Dust = (float)Math.Round((float)_emulatorRandom.NextDouble() * 350, 3),
                Temp = (float)Math.Round((float)_emulatorRandom.NextDouble() * 40, 3),
                Preassure = (float)Math.Round((float)_emulatorRandom.NextDouble() * 20, 3),
                Hum = (float)Math.Round((float)_emulatorRandom.NextDouble() * 40, 3),
                Created = DateTime.UtcNow - new TimeSpan(0, 0, 10),
                TrackingKey = trackingKey
            };
        }


        private static Sensor GetFakeSensor()
        {
            var fakeLatLong = GetLocation(27.560597, 53.904588, 8000);
            var sensor = new Sensor
            {
                Latitude = fakeLatLong.randomLatitude,
                Longitude = fakeLatLong.randomLongitude,
                TrackingKey = Guid.NewGuid().ToString()
            };
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