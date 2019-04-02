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

        //public static async Task<Sensor> GetSensorByTrackingKeyAsync(string trackingKey)
        //{
        //    using (var _context = new DataContext())
        //    {
        //        return await _context.Sensors.AsNoTracking().FirstOrDefaultAsync(f => f.TrackingKey == trackingKey);
        //    }
        //}

        public static async Task<Sensor> AddSensorAsync(string ipAddress, SensorType type, double? latitude = null, double? longitude = null)
        {
            using (var _context = new DataContext())
            {
                var sensor = new Sensor
                {
                    Readings = new List<Reading>(),
                    IPAddress = ipAddress,
                    Type = type
                };
                if (sensor.Type == SensorType.Static)
                {
                    sensor.Latitude = latitude;
                    sensor.Longitude = longitude;
                }
                _context.Sensors.Add(sensor);
                await _context.SaveChangesAsync();
                return sensor;
            }
        }

        public static async Task<Sensor> UpdateSensorCoordinates(int id, double latitude, double longitude)
        {
            using (var _context = new DataContext())
            {
                var sensor = await _context.Sensors.FirstOrDefaultAsync(f => f.Id == id);
                sensor.Latitude = latitude;
                sensor.Longitude = longitude;
                _context.Entry(sensor).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return sensor;
            }
        }

        public static async Task<List<Sensor>> GetSensorsAsync(bool withReadings)
        {
            using (var context = new DataContext())
            {
                var query = context
                      .Sensors.Where(f => !f.IsDeleted);
                if (withReadings)
                {
                    query = query.IncludeFilter(f => f.Readings
                                            .OrderByDescending(s => s.Created)
                                            .Take(10));
                }
                var sensors = await query.ToListAsync();
                return sensors;
            }
        }


        public static async Task RemoveSensorAsync(int id)
        {
            using (var _context = new DataContext())
            {
                await _context.Sensors.Where(f => f.Id == id).UpdateAsync(f => new Sensor
                {
                    IsDeleted = true
                });
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
                return reading;
            }
        }

        //#region Cache

        //public static async Task<List<SensorCacheItemModel>> GetSensorsFromCache()
        //{
        //    throw new NotImplementedException();
        //}

        //private static async Task AddSensorToCacheAsync(Sensor sensor)
        //{
        //    throw new NotImplementedException();
        //}


        //public static async Task UpdateSensorCacheWithReadingAsync(Reading reading)
        //{
        //    if (reading.SensorId <= 0)
        //    {
        //        throw new ArgumentException("SensorId should be provided in reading!", nameof(reading.SensorId));
        //    }
        //    var sensorsCacheItems = await GetOrAddSensorsToCacheAsync();
        //    var sensorInCache = sensorsCacheItems.FirstOrDefault(f => f.Sensor.Id == reading.SensorId);
        //    if (sensorInCache == null)
        //    {
        //        throw new KeyNotFoundException();
        //    }
        //    sensorInCache.Sensor.Readings.Add(reading);
        //    sensorInCache.Sensor.Readings = sensorInCache.Sensor.Readings.OrderByDescending(f => f.Created).Take(10).ToList();
        //    sensorInCache.PollutionLevel = PollutionHelper.CalculatePollutionLevel(sensorInCache.Sensor.Readings);
        //    CacheHelper.AddOrUpdate(SensorsCacheKey, sensorsCacheItems, DateTime.UtcNow.Add(SensorsCacheLifetime));
        //}
        //#endregion
    }
}