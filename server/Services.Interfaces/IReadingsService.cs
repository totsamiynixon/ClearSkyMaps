using Services.DTO.Models.Reading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IReadingService
    {
        Task<SaveReadingResultDTO> SaveReadingAsync(string trackingKey, SaveReadingDTO reading);

        Task<List<SensorReadingDTO>> GetReadingsForExportAsync(DateTime startPeriod, DateTime endPeriod, int sensorId, int? everyNth = null);
    }
}
