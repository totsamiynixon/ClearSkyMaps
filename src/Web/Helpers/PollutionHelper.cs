using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Data.Models;
using Web.Enum;
using Web.Models.Cache;

namespace Web.Helpers
{
    public static class PollutionHelper
    {
        private static Random _random = new Random();
        public static PollutionLevel? GetPollutionLevel(int sensorId)
        {

            var sensorsInCache = CacheHelper.Get<List<SensorCacheItemModel>>(DatabaseHelper.SensorsCacheKey);
            if (sensorsInCache == null)
            {
                throw new InvalidOperationException("There are no sensors in cache!");
            }
            var sensor = sensorsInCache.FirstOrDefault(f => f.Sensor.Id == sensorId);
            if (sensor == null)
            {
                throw new KeyNotFoundException("There are no such sensor in cache!");
            }
            return sensor.PollutionLevel;
        }


        public static PollutionLevel? CalculatePollutionLevel(List<Reading> readings)
        {
            if (!readings.Any())
            {
                return null;
            }
            return (PollutionLevel)_random.Next(0, 2);
        }
    }
}