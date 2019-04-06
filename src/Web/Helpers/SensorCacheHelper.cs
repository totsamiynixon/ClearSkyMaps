using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Web.Data.Models;
using Web.Enum;
using Web.Models.Cache;

namespace Web.Helpers
{
    public static class SensorCacheHelper
    {
        private const string _sensorCacheKey = "CSMStaticSensors";
        private static TimeSpan _sensorCacheLifetime => new TimeSpan(1, 0, 0);

        public static async Task<List<SensorCacheItemModel>> GetStaticSensorsAsync()
        {
            return await CacheHelper.GetOrAddAsync(_sensorCacheKey, async () =>
            {
                List<SensorCacheItemModel> result = new List<SensorCacheItemModel>();
                var sensors = await DatabaseHelper.GetStaticSensorsForCacheAsync();
                foreach (var sensor in sensors)
                {
                    result.Add(new SensorCacheItemModel(sensor, PollutionHelper.CalculatePollutionLevel(sensor.Readings)));
                }
                return result;
            }, DateTime.UtcNow.Add(_sensorCacheLifetime));
        }

        public static async Task UpdateStaticSensorCache(StaticSensor sensor)
        {
            if (sensor.IsAvailable())
            {
                await AddStaticSensorToCacheAsync(sensor.Id);
            }
            else
            {
                await RemoveStaticSensorFromCacheAsync(sensor.Id);
            }
        }

        public static async Task AddStaticSensorToCacheAsync(int sensorId)
        {
            var sensorsCacheItems = await GetStaticSensorsAsync();
            var sensorInCache = sensorsCacheItems.FirstOrDefault(f => f.Sensor.Id == sensorId);
            if (sensorInCache == null)
            {
                var sensor = await DatabaseHelper.GetStaticSensorByIdAsync(sensorId, true);
                sensorsCacheItems.Add(new SensorCacheItemModel(sensor, PollutionHelper.CalculatePollutionLevel(sensor.Readings)));
                CacheHelper.AddOrUpdate(_sensorCacheKey, sensorsCacheItems, DateTime.UtcNow.Add(_sensorCacheLifetime));
            }
        }

        public static async Task RemoveStaticSensorFromCacheAsync(int sensorId)
        {
            var sensorsCacheItems = await GetStaticSensorsAsync();
            var sensorInCache = sensorsCacheItems.FirstOrDefault(f => f.Sensor.Id == sensorId);
            if (sensorInCache != null)
            {
                sensorsCacheItems.Remove(sensorInCache);
                CacheHelper.AddOrUpdate(_sensorCacheKey, sensorsCacheItems, DateTime.UtcNow.Add(_sensorCacheLifetime));
            }
        }

        public static void RemoveAllSensorsFromCache()
        {
            CacheHelper.Remove(_sensorCacheKey);
        }


        public static async Task UpdateSensorCacheWithReadingAsync(Reading reading)
        {
            if (reading.SensorId <= 0)
            {
                throw new ArgumentException("SensorId should be provided in reading!", nameof(reading.SensorId));
            }
            var sensorsCacheItems = await GetStaticSensorsAsync();
            var sensorInCache = sensorsCacheItems.FirstOrDefault(f => f.Sensor.Id == reading.SensorId);
            if (sensorInCache == null)
            {
                throw new KeyNotFoundException();
            }
            sensorInCache.Sensor.Readings.Add(reading);
            sensorInCache.Sensor.Readings = sensorInCache.Sensor.Readings.OrderByDescending(f => f.Created).Take(10).ToList();
            sensorInCache.PollutionLevel = PollutionHelper.CalculatePollutionLevel(sensorInCache.Sensor.Readings);
            CacheHelper.AddOrUpdate(_sensorCacheKey, sensorsCacheItems, DateTime.UtcNow.Add(_sensorCacheLifetime));
        }

        public static async Task<PollutionLevel> GetPollutionLevelAsync(int sensorId)
        {
            var sensorsCacheItems = await GetStaticSensorsAsync();
            var sensorInCache = sensorsCacheItems.FirstOrDefault(f => f.Sensor.Id == sensorId);
            if (sensorInCache == null)
            {
                throw new KeyNotFoundException();
            }
            return sensorInCache.PollutionLevel;
        }
    }
}