using AutoMapper;
using Readings.DAL.Intarfaces;
using Readings.Domain;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Readings.Services.DTO.Models.Reading;
using Readings.Services.DTO.Sensor;
using Readings.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Readings.API.Controllers.Api;
using Readings.API.Hubs;
using Readings.API.Models.Api.Readings;

namespace Readings.API.Emulator
{
    public class Emulator
    {
        private static Random _emulatorRandom = new Random();
        private static double memoryLimit = 8e+8;

        private List<string> _trackingKeys;
        private readonly IServiceProvider _resolver;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public Emulator(IServiceProvider serviceProvider, IConfiguration configuration, Func<Type, IMapper> serviceAccessor)
        {
            _resolver = serviceProvider;
            _configuration = configuration;
            _mapper = serviceAccessor(typeof(Emulator));
        }

        public bool IsEmulationEnabled => _configuration.GetValue<bool?>("emulationEnabled") ?? false;

        public async Task RunEmulationAsync()
        {
            if (!IsEmulationEnabled)
            {
                _configuration["emulationEnabled"] = bool.TrueString.ToLower();
                await SeedSensorsAsync();
                StartTask();
            }
        }

        public void SetMemoryLimit(double limit)
        {
            memoryLimit = limit;
        }
        public async Task StopEmulationAsync()
        {
            if (IsEmulationEnabled)
            {
                _configuration["emulationEnabled"] = bool.FalseString.ToLower();
                await ClearSensorsAsync();
            }
        }

        private void StartTask()
        {
            var task = Task.Run(async () =>
            {
                while (IsEmulationEnabled)
                {
                    try
                    {
                        await DispatchFakeDataAsync();
                    }
                    catch (Exception ex)
                    {
                        await StopEmulationAsync();
                    }
                }
            });
        }

        private async Task SeedSensorsAsync()
        {
            var databaseMigrator = (IDatabaseMigrator)_resolver.GetService(typeof(IDatabaseMigrator));
            databaseMigrator.Migrate();
            await ClearSensorsAsync();
            var service = (ISensorService)_resolver.GetService(typeof(ISensorService));
            _trackingKeys = new List<string>();
            var iterations = _emulatorRandom.Next(1, 20);
            for (int i = 0; i < iterations; i++)
            {
                var sensorKey = await service.RegisterAndGetTrackingKeyAsync(GetFakeSensor());
                _trackingKeys.Add(sensorKey);
            }
        }

        private async Task ClearSensorsAsync()
        {
            var context = (IDataContext)_resolver.GetService(typeof(IDataContext));
            var repo = context.GetRepository<Sensor>();
            repo.ToList().ForEach(s => repo.Remove(s));
            await context.SaveChangesAsync();
            _trackingKeys = null;
        }

        private async Task DispatchFakeDataAsync()
        {

            int delayTime = 10000 / _trackingKeys.Count;
            foreach (var key in _trackingKeys)
            {
                var controller = new ReadingsController((IReadingService)_resolver.GetService(typeof(IReadingService)), (ISensorService)_resolver.GetService(typeof(ISensorService)), (IHubContext<ReadingsHub, IReadingsClient>)_resolver.GetService(typeof(IHubContext<ReadingsHub, IReadingsClient>)), (Func<Type, IMapper>)_resolver.GetService(typeof(Func<Type, IMapper>)));
                var fakeReading = GetFakeReading();
                var fakeReadingApiModel = _mapper.Map<SaveReadingDTO, ApiPostReadingModel>(fakeReading);
                fakeReadingApiModel.SensorTrackingKey = key;
                await controller.PostReading(fakeReadingApiModel);
                await Task.Delay(delayTime);
            }
        }

        private SaveReadingDTO GetFakeReading()
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
            };
        }


        private RegisterSensorDTO GetFakeSensor()
        {
            var fakeLatLong = GetLocation(27.560597, 53.904588, 8000);
            var sensor = new RegisterSensorDTO
            {
                Latitude = fakeLatLong.randomLatitude,
                Longitude = fakeLatLong.randomLongitude,
            };
            return sensor;
        }

        private (double randomLongitude, double randomLatitude) GetLocation(double longitude, double latittude, int radius)
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