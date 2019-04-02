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
        public static string SensorsCacheKey => "StaticSensors";
        private static TimeSpan SensorsCacheLifetime => new TimeSpan(1, 0, 0);

        public static async Task<StaticSensor> AddStaticSensorAsync(string ipAddress, double latitude, double longitude)
        {
            using (var _context = new DataContext())
            {
                var sensor = new StaticSensor
                {
                    Readings = new List<Reading>(),
                    IPAddress = ipAddress,
                };
                sensor.Latitude = latitude;
                sensor.Longitude = longitude;
                _context.StaticSensors.Add(sensor);
                await _context.SaveChangesAsync();
                return sensor;
            }
        }

        public static async Task<PortableSensor> AddPortableSensor(string ipAddress)
        {
            using (var _context = new DataContext())
            {
                var sensor = new PortableSensor
                {
                    IPAddress = ipAddress
                };
                _context.PortableSensors.Add(sensor);
                await _context.SaveChangesAsync();
                return sensor;
            }
        }

        public static async Task<StaticSensor> UpdateStaticSensorCoordinates(int id, double latitude, double longitude)
        {
            using (var _context = new DataContext())
            {
                var staticSensor = await _context.StaticSensors.FirstOrDefaultAsync(f => f.Id == id);
                staticSensor.Latitude = latitude;
                staticSensor.Longitude = longitude;
                _context.Entry(staticSensor).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return staticSensor;
            }
        }

        public static async Task<StaticSensor> UpdateStaticSensorVisibility(int id, bool value)
        {
            using (var _context = new DataContext())
            {
                var staticSensor = await _context.StaticSensors.FirstOrDefaultAsync(f => f.Id == id);
                staticSensor.IsVisible = value;
                _context.Entry(staticSensor).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return staticSensor;
            }
        }

        public static async Task<Sensor> ChangeSensorActivationAsync(int id, bool value)
        {
            using (var _context = new DataContext())
            {
                var sensor = await _context.Sensors.FirstOrDefaultAsync(f => f.Id == id);
                sensor.IsActive = value;
                _context.Entry(sensor).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return sensor;
            }
        }

        public static async Task<List<Sensor>> GetSensorsAsync()
        {
            using (var context = new DataContext())
            {
                var query = context
                      .Sensors.Where(f => !f.IsDeleted);
                var sensors = await query.ToListAsync();
                return sensors;
            }
        }


        public static async Task<List<StaticSensor>> GetStaticSensorsAsync(bool withReadings)
        {
            using (var context = new DataContext())
            {
                var query = context
                      .StaticSensors.Where(f => !f.IsDeleted);
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

        public static async Task<Sensor> GetSensorByIdAsync(int id)
        {
            using (var context = new DataContext())
            {
                return await context.Sensors.FirstOrDefaultAsync(f => f.Id == id);
            }
        }


        public static async Task RemoveSensorAsync(int id)
        {
            using (var _context = new DataContext())
            {
                await _context.Sensors.Where(f => f.Id == id).UpdateAsync(f => new StaticSensor
                {
                    IsDeleted = true
                });
            }
        }

        public static async Task RemoveSensorsFromDatabaseAsync(params int[] ids)
        {
            using (var _context = new DataContext())
            {
                await _context.Sensors.Where(f => ids.Any(s => s == f.Id)).DeleteAsync();
            }
        }

        public static async Task RemoveAllSensorsFromDatabaseAsync()
        {
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