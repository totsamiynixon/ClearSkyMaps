using DAL.Implementations;
using DAL.Implementations.Contexts;
using Domain;
using Services.DTO.Reading;
using Services.DTO.Sensor;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class SensorService : ISensorService
    {
        private readonly DataContext _db;
        private readonly DbSet<Sensor> _sensorRepository;
        public SensorService(DataContext dbcontext)
        {
            _db = dbcontext;
            _sensorRepository = _db.GetRepository<Sensor>();
        }
        /// <summary>
        /// Checks does sensor with provided tracking key exist.
        /// </summary>
        /// <param name="trackingKey"> Sensor's tracking key </param>
        /// <returns></returns>
        public Task<bool> CheckTrackingKeyAsync(string trackingKey)
        {
            return _sensorRepository.AnyAsync(f => f.TrackingKey == trackingKey);
        }

        public Task<int> GetSensorIdByTrackingKeyAsync(string trackingKey)
        {
            return _sensorRepository.Where(s => s.TrackingKey == trackingKey).Select(f => f.Id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Returns list of records with provided length.
        /// </summary>
        /// <param name="count">Count of records</param>
        /// <returns> List of sensors with readings </returns>
        public Task<List<SensorInfoDTO>> GetSensorListAsync(int count)
        {
            return _sensorRepository.Select(f => new SensorInfoDTO
            {
                Id = f.Id,
                Latitude = f.Latitude,
                Longitude = f.Longitude,
                Readings = f.Readings.Where(z => z.Created.Day == DateTime.UtcNow.Day).OrderByDescending(z => z.Created).Select(z => new SensorReadingDTO
                {
                    CH4 = z.CH4,
                    CO = z.CO,
                    CO2 = z.CO2,
                    Dust = z.Dust,
                    Hum = z.Hum,
                    LPG = z.LPG,
                    Preassure = z.Preassure,
                    Temp = z.Temp,
                    Created = z.Created,
                }).Take(count).ToList()
            }).ToListAsync();
        }
        /// <summary>
        /// Every sensor should be registered in system to be able to send data. Use this method to do it.
        /// </summary>
        /// <param name="sensor">Object with data about sensor</param>
        /// <returns> Tracking key for registered sensor </returns>
        public async Task<string> RegisterAndGetTrackingKeyAsync(RegisterSensorDTO sensor)
        {
            var entity = new Sensor
            {
                Latitude = sensor.Latitude,
                Longitude = sensor.Longitude,
                TrackingKey = Guid.NewGuid().ToString()
            };
            _sensorRepository.Add(entity);
            await _db.SaveChangesAsync();
            return entity.TrackingKey;
        }
    }
}
