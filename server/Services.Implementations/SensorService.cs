using AutoMapper;
using DAL.Intarfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Services.DTO.Enums;
using Services.DTO.PollutionCalculator;
using Services.DTO.Reading;
using Services.DTO.Sensor;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class SensorService : ISensorService
    {
        private readonly IDataContext _db;
        private readonly DbSet<Sensor> _sensorRepository;
        private readonly IPollutionLevelCalculationService _pollutionLevelCalculationService;
        private readonly IMapper _mapper;
        public SensorService(
            IDataContext dbcontext,
            IPollutionLevelCalculationService pollutionLevelCalculationService,
            IMapper mapper)
        {
            _db = dbcontext;
            _sensorRepository = _db.GetRepository<Sensor>();
            _mapper = mapper;
            _pollutionLevelCalculationService = pollutionLevelCalculationService;
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


        public async Task<(int sensorId, PollutionLevel level)> GetSensorPollutionLevelInfoAsync(string trackingKey)
        {
            var sensor = await _sensorRepository.Where(s => s.TrackingKey == trackingKey).Select(f => new { Id = f.Id, Readings = f.Readings.Take(10).ToArray()}).FirstOrDefaultAsync();
            return (sensor.Id, _pollutionLevelCalculationService.Calculate(_mapper.Map<IEnumerable<Reading>, ReadingForPollutionCalculation[]>(sensor.Readings)));
        }

        /// <summary>
        /// Returns list of records with provided length.
        /// </summary>
        /// <returns> List of sensors with readings </returns>
        public async Task<List<SensorInfoDTO>> GetSensorListAsync()
        {
            var sensors = await _sensorRepository.Select(f => new SensorInfoDTO
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
                }).Take(10).ToList()
            }).ToListAsync();
            Parallel.ForEach(sensors, (sensor) =>
            {
                sensor.LatestPollutionLevel = _pollutionLevelCalculationService.Calculate(_mapper.Map<List<SensorReadingDTO>, ReadingForPollutionCalculation[]>(sensor.Readings));
            });
            return sensors;
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
