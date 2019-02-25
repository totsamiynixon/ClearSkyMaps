using AutoMapper;
using Readings.DAL.Intarfaces;
using Readings.Domain;
using Microsoft.EntityFrameworkCore;
using Readings.Services.DTO.Enums;
using Readings.Services.DTO.PollutionCalculator;
using Readings.Services.DTO.Models.Reading;
using Readings.Services.DTO.Sensor;
using Readings.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readings.Services.Implementations
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

        public Task<bool> CheckTrackingKeyAsync(string trackingKey)
        {
            return _sensorRepository.AnyAsync(f => f.TrackingKey == trackingKey);
        }

        public Task<int> GetSensorIdByTrackingKeyAsync(string trackingKey)
        {
            return _sensorRepository.Where(s => s.TrackingKey == trackingKey).Select(f => f.Id).FirstOrDefaultAsync();
        }

        public async Task<PollutionLevel> GetSensorPollutionLevelInfoAsync(string trackingKey)
        {
            var sensor = await _sensorRepository.Where(s => s.TrackingKey == trackingKey).Select(f => new { Id = f.Id, Readings = f.Readings.Take(10).ToArray() }).FirstOrDefaultAsync();
            return (_pollutionLevelCalculationService.Calculate(_mapper.Map<IEnumerable<Reading>, ReadingForPollutionCalculation[]>(sensor.Readings)));
        }

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
