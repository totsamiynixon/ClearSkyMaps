using AutoMapper;
using DAL.Implementations.Contexts;
using Domain;
using Services.DTO.Reading;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class ReadingService : IReadingService
    {
        private readonly DbSet<Reading> _readingRepository;
        private readonly DbSet<Sensor> _sensorRepository;
        private readonly DataContext _db;
        public ReadingService(DataContext context)
        {
            _db = context;
            _readingRepository = _db.GetRepository<Reading>();
            _sensorRepository = _db.GetRepository<Sensor>();
        }

        public async Task<List<SensorReadingDTO>> GetReadingsForExportAsync(DateTime startPeriod, DateTime endPeriod, int sensorId, int? everyNth = null)
        {
            var items = await _readingRepository.Where(f => f.Created >= startPeriod && f.Created < endPeriod && f.SensorId == sensorId && !everyNth.HasValue || f.Id % everyNth.Value == 0).ToListAsync();
            return Mapper.Map<List<Reading>, List<SensorReadingDTO>>(items);
        }

        /// <summary>
        /// Attachs reading to sensor and saves in storage.
        /// </summary>
        /// <param name="trackingKey"> Owner sensor tracking key for detect which reading it is </param>
        /// <param name="readingModel"> Object with reading's data </param>
        /// <returns></returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Thrown where there are no such sensors with provided tracking key 
        /// and the other is greater than 0.</exception>
        public async Task SaveReadingAsync(string trackingKey, SaveReadingDTO readingModel)
        {
            var sensor = await _sensorRepository.FirstOrDefaultAsync(s => s.TrackingKey == trackingKey);
            if (sensor == null)
            {
                throw new KeyNotFoundException("Tracking key doesn't exist! Register sensor first, please!");
            }
            var entity = Mapper.Map<SaveReadingDTO, Reading>(readingModel);
            entity.SensorId = sensor.Id;
            _readingRepository.Add(entity);
            await _db.SaveChangesAsync();
        }
    }
}
