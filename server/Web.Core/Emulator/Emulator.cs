using ArduinoServer.Controllers.Api;
using DAL.Intarfaces;
using Domain;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Services.DTO.Reading;
using Services.DTO.Sensor;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Web.Core.Controllers.Api;
using Web.Core.Hubs;

namespace Web.Core.Emulator
{
    public class Emulator
    {
        private static Random _emulatorRandom = new Random();
        private static double memoryLimit = 5e+8;

        private List<string> _trackingKeys;
        private readonly IServiceProvider _resolver;
        private readonly IConfiguration _configuration;
        public Emulator(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _resolver = serviceProvider;
            _configuration = configuration;
        }

        public bool IsEmulationEnabled => _configuration.GetValue<bool?>("emulationEnabled") ?? false;

        public void RunEmulation()
        {
            if (!IsEmulationEnabled)
            {
                _configuration["emulationEnabled"] = bool.TrueString.ToLower();
                SeedSensors();
                StartTask();
            }
        }

        public void SetMemoryLimit(double limit)
        {
            memoryLimit = limit;
        }
        public void StopEmulation()
        {
            if (IsEmulationEnabled)
            {
                _configuration["emulationEnabled"] = bool.FalseString.ToLower();
                ClearSensors();
            }
        }

        private void StartTask()
        {
            var task = Task.Run(async () =>
            {
                while (IsEmulationEnabled)
                {
                    DispatchFakeData();
                    await Task.Delay(10000);
                    try
                    {
                        CheckMemory();
                    }
                    catch (OutOfMemoryException ex)
                    {
                        StopEmulation();
                    }
                }
            });
        }

        private async void SeedSensors()
        {
            var service = (ISensorService)_resolver.GetService(typeof(ISensorService));
            _trackingKeys = new List<string>();
            var iterations = _emulatorRandom.Next(1, 20);
            for (int i = 0; i < iterations; i++)
            {
                _trackingKeys.Add(await service.RegisterAndGetTrackingKeyAsync(GetFakeSensor()));
            }
        }

        private async void ClearSensors()
        {
            var context = (IDataContext)_resolver.GetService(typeof(IDataContext));
            var repo = context.GetRepository<Sensor>();
            repo.ToList().ForEach(s => repo.Remove(s));
            await context.SaveChangesAsync();
            _trackingKeys = null;
        }

        private  void DispatchFakeData()
        {
            Parallel.ForEach(_trackingKeys, async (key) =>
            {
                var controller = new ReadingsController((IReadingService)_resolver.GetService(typeof(IReadingService)), (ISensorService)_resolver.GetService(typeof(ISensorService)), (IHubContext<ReadingsHub,IReadingsClient>)_resolver.GetService(typeof(IHubContext<ReadingsHub, IReadingsClient>)));
                var fakeReading = GetFakeReading();
                await controller.PostReading(key, fakeReading);
            });
        }

        private  SaveReadingDTO GetFakeReading()
        {
            return new SaveReadingDTO
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


        private  RegisterSensorDTO GetFakeSensor()
        {
            var fakeLatLong = GetLocation(27.560597, 53.904588, 8000);
            var sensor = new RegisterSensorDTO
            {
                Latitude = fakeLatLong.randomLatitude,
                Longitude = fakeLatLong.randomLongitude,
            };
            return sensor;
        }

        private  (double randomLongitude, double randomLatitude) GetLocation(double longitude, double latittude, int radius)
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

        private  void CheckMemory()
        {
            Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            long totalBytesOfMemoryUsed = currentProcess.WorkingSet64;
            if (totalBytesOfMemoryUsed > memoryLimit)
            {
                throw new OutOfMemoryException("Too much memory used!");
            }
        }
    }
}