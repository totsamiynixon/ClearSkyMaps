using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Web.Data;
using Web.Data.Models;
using Web.Enum;
using Web.Models.Cache;
using Z.EntityFramework.Plus;

namespace Web.Helpers
{
    public static class DatabaseHelper
    {
        public static string SensorsCacheKey => "Sensors";
        private static TimeSpan SensorsCacheLifetime => new TimeSpan(1, 0, 0);

        public static async Task<Sensor> GetSensorByTrackingKeyAsync(string trackingKey)
        {
            using (var _context = new DataContext())
            {
                return await _context.Sensors.AsNoTracking().FirstOrDefaultAsync(f => f.TrackingKey == trackingKey);
            }
        }

        public static async Task<Sensor> AddSensorAsync(double latitude, double longitude)
        {
            using (var _context = new DataContext())
            {
                var sensor = new Sensor
                {
                    Latitude = latitude,
                    Longitude = longitude,
                    TrackingKey = Guid.NewGuid().ToString(),
                    Readings = new List<Reading>()
                };
                _context.Sensors.Add(sensor);
                await _context.SaveChangesAsync();
                await AddSensorToCacheAsync(sensor);
                return sensor;
            }
        }

        public static async Task<List<Sensor>> GetSensorsAsync()
        {
            return (await GetOrAddSensorsToCacheAsync()).Select(f => f.Sensor).ToList();
        }

        public static async Task RemoveAllSensorsAsync()
        {
            CacheHelper.Remove(SensorsCacheKey);
            using (var _context = new DataContext())
            {
                await _context.Sensors.Where(f => true).DeleteAsync();
            }
        }

        public static async Task<Reading> AddReadingAsync(Reading reading)
        {
            if (reading.SensorId <= 0)
            {
                throw new ArgumentException("SensorId should be provided in reading!", nameof(reading.SensorId));
            }
            using (var _context = new DataContext())
            {
                _context.Readings.Add(reading);
                await _context.SaveChangesAsync();
                await UpdateSensorCacheWithReadingAsync(reading);
                return reading;
            }
        }

        #region Cache

        private static async Task<List<SensorCacheItemModel>> GetOrAddSensorsToCacheAsync()
        {
            return await CacheHelper.GetOrAddAsync(SensorsCacheKey, async () =>
            {
                using (var context = new DataContext())
                {
                    List<SensorCacheItemModel> result = new List<SensorCacheItemModel>();
                    var sensors = await context
                          .Sensors
                          .IncludeFilter(f => f.Readings
                                               .OrderByDescending(s => s.Created)
                                               .Take(10))
                          .ToListAsync();
                    foreach (var sensor in sensors)
                    {
                        result.Add(new SensorCacheItemModel(sensor, PollutionHelper.CalculatePollutionLevel(sensor.Readings)));
                    }
                    return result;
                }
            }, DateTime.UtcNow.Add(SensorsCacheLifetime));
        }

        private static async Task AddSensorToCacheAsync(Sensor sensor)
        {
            var sensorsCacheItems = await GetOrAddSensorsToCacheAsync();
            var sensorInCache = sensorsCacheItems.FirstOrDefault(f => f.Sensor.Id == sensor.Id);
            if (sensorInCache == null)
            {
                sensorsCacheItems.Add(new SensorCacheItemModel(sensor, PollutionHelper.CalculatePollutionLevel(sensor.Readings)));
                CacheHelper.AddOrUpdate(SensorsCacheKey, sensorsCacheItems, DateTime.UtcNow.Add(SensorsCacheLifetime));
            }
        }


        public static async Task UpdateSensorCacheWithReadingAsync(Reading reading)
        {
            if (reading.SensorId <= 0)
            {
                throw new ArgumentException("SensorId should be provided in reading!", nameof(reading.SensorId));
            }
            var sensorsCacheItems = await GetOrAddSensorsToCacheAsync();
            var sensorInCache = sensorsCacheItems.FirstOrDefault(f => f.Sensor.Id == reading.SensorId);
            if (sensorInCache == null)
            {
                throw new KeyNotFoundException();
            }
            sensorInCache.Sensor.Readings.Add(reading);
            sensorInCache.Sensor.Readings = sensorInCache.Sensor.Readings.OrderByDescending(f => f.Created).Take(10).ToList();
            sensorInCache.PollutionLevel = PollutionHelper.CalculatePollutionLevel(sensorInCache.Sensor.Readings);
            CacheHelper.AddOrUpdate(SensorsCacheKey, sensorsCacheItems, DateTime.UtcNow.Add(SensorsCacheLifetime));
        }
        #endregion
    }
}